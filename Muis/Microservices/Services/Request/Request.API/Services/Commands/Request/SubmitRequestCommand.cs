using Core.API;
using Request.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Request.API.Repository;
using Core.API.Repository;
using Core.Model;
using Core.EventBus;
using Request.Events;
using Request.API.Strategies.Certificate360;


namespace Request.API.Services.Commands.Request
{
  public class SubmitRequestCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly Model.Request _request;
    readonly IEventBus _eventBus;

    public SubmitRequestCommand(Model.Request request, IEventBus eventBus)
    {
      _request = request;
      _eventBus = eventBus;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      //Added the below code for check the customer already exists. If not insert the new customer to Request DB
      if (_request.CustomerID != null)
      {
        var existCustomer = dbContext.Customer.GetByID(_request.CustomerID).Result;
        if (existCustomer == null)
        {
          Model.Customer orgCustomer = new Model.Customer();
          orgCustomer.ID = _request.CustomerID;
          orgCustomer.Name = _request.CustomerName;
          await dbContext.Customer.InsertCustomer(orgCustomer);

          var newCustomer = dbContext.Customer.GetByID(_request.CustomerID).Result;
          if (newCustomer != null)
          {
            _request.CustomerID = newCustomer.ID;
            _request.CustomerName = newCustomer.Name;
            if (newCustomer.CodeID == null )
            {
              _request.Status = RequestStatus.Draft;
              _request.StatusMinor = RequestStatusMinor.PendingCustomerCode;
            }
          }
        }
      }
      // Get Parent Request and assign parent id
      Model.Request parentRequest = GetParentRequest(_request, dbContext);
      if (parentRequest != null && parentRequest.ID > 0)
      {
        _request.ParentID = parentRequest.ID;
      }

      if (_request.Type == RequestType.HC02 || _request.Type == RequestType.Renewal)
      {
        if (_request.Characteristics != null)
        {
          var characteristic = CharacteristicWithCertificate(
            _request.Characteristics);

          if (characteristic != null)
          {
            long oldrequestID = dbContext.Request.GetRequestIDFromCharacteristic(
              characteristic).Result;

            Model.Request oldrequest = dbContext.Request.GetRequestByID(
              oldrequestID).Result;

            if (oldrequest != null)
            {
              //_request.ParentID = oldrequest.ID;

              UpdateChangeFlags(oldrequest, _request);
            }
          }
        }
      }

      if (_request.Type != RequestType.HC01)
      {
        if (_request.Status == RequestStatus.Draft
          && _request.StatusMinor == RequestStatusMinor.PendingCustomerCode)
        {
          // send Push notification to all users who has the Customer Code permission.
          await SendNotificationWithPermission(dbContext);
        }
        else
        {
          Officer user = new Officer
          {
            ID = (Guid)_request.AssignedTo,
            Name = _request.AssignedToName
          };
          await dbContext.User.InsertOrReplace(user);

          if (_request.AssignedTo != null)
          {
            await SendNotificationToAssignedOfficer(dbContext, user);
          }
          else
          {
            // Send Push Notification to admin
            await SendNotificationToAdmin(dbContext);
          }
        }
      }

      var id = await dbContext.Request.InsertRequest(_request);

      var result = await dbContext.Request.GetRequestByID(id);

      if (result.Type == RequestType.HC01)
      {
        //await new Certificate360Strategy
        //  (dbContext, result, new Certificate { Number = "" }, null).Invoke();
        var logText = string.Format(
       await dbContext.Transalation.GetTranslation(0, "SystemProcessedRequest"));

        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = logText
        });

        await dbContext.Request.MapLog(id, logID);
      }

      unitOfWork.Commit();

      return result;
    }

    private async Task SendNotificationWithPermission(DbContext dbContext)
    {
      _eventBus.Publish(new SendNotificationWithPermissionsEvent
      {
        Title = await dbContext.Transalation.GetTranslation
       (Locale.EN, "CustomerCodeAssignTitle"),
        Body = string.Format(await dbContext.Transalation.GetTranslation
       (Locale.EN, "CustomerCodeAssignBody")),
        Module = "Request",
        RefID = $"{_request.RefID}",
        Permissions = new Permission[] {
            Permission.SetupCustomerCode
          }       
      });
    }

    private Model.Request GetParentRequest(Model.Request request, DbContext dbContext)
    {
      Model.Request pRequest = null;

      if (request != null && request.LineItems != null && request.LineItems.Count > 0)
      {
        var scheme = request.LineItems[0].Scheme;
        var subSucheme = request.LineItems[0].SubScheme;
        var prem = request.Premises.Where(p => p.IsPrimary == true).FirstOrDefault();
        if (request.Type == RequestType.New)
        {
          RequestStatus[] statuses = new RequestStatus[] { RequestStatus.Expired };
          RequestType[] reqTypes = new RequestType[] { RequestType.New, RequestType.HC02,
            RequestType.Renewal };
          pRequest = dbContext.Request.GetParentRequest
            (scheme, subSucheme, prem, statuses, reqTypes).Result;
        }
        else
        {
          RequestStatus[] statuses = new RequestStatus[] { RequestStatus.Closed };
          RequestType[] reqTypes = new RequestType[] { RequestType.New, RequestType.HC02,
            RequestType.Renewal };
          pRequest = dbContext.Request.GetParentRequest
            (scheme, subSucheme, prem, statuses, reqTypes).Result;
        }
      }
      return pRequest;
    }

    private async Task SendNotificationToAdmin(DbContext dbContext)
    {
      string postalcode = string.Empty;
      var outlet = _request.Premises.Where(o => o.Type == PremiseType.Outlet).FirstOrDefault();
      if (outlet != null)
      {
        postalcode = outlet.Postal;
      }
      _eventBus.Publish(new SendNotificationWithPermissionsEvent
      {
        Title = await dbContext.Transalation.GetTranslation
        (Locale.EN, "AssignCANotFoundNotifTitle"),
        Body = string.Format(await dbContext.Transalation.GetTranslation
        (Locale.EN, "AssignCANotFoundNotifBody"), postalcode),
        Module = "Request",
        RefID = $"{_request.RefID}",
        Permissions = new Permission[] {
            Permission.RequestReassign,
            Permission.RequestOverride
          },
        Excludes = new string[] { "" }
      });
    }

    private async Task SendNotificationToAssignedOfficer(DbContext dbContext, Officer user)
    {
      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationTitle"),
        Body = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationBody"),
        Module = "Request",
        To = new List<string> { user.ID.ToString() }
      });
    }

    private void UpdateChangeFlags(
      Model.Request oldrequest,
      Model.Request request)
    {
      if (request.Type != RequestType.HC01)
      {
        UpdateMenusChangeFlags(oldrequest, request);
        UpdateIngredientChangeFlags(oldrequest, request);
      }
      UpdatePremiseChangeFlags(oldrequest, request);
      UpdateHalalTeamChangeFlags(oldrequest, request);

    }

    private void UpdateMenusChangeFlags(Model.Request oldrequest, Model.Request request)
    {

      if (oldrequest.Menus != null && oldrequest.Menus.Count > 0 &&
  request.Menus != null && request.Menus.Count > 0)
      {
        oldrequest.Menus = oldrequest.Menus.Where
          (m => m.ChangeType != ChangeType.Delete).ToList();

        foreach (var newMenu in request.Menus)
        {
          var item = oldrequest.Menus.Where(m => m.Text.Trim().ToLower() ==
          newMenu.Text.Trim().ToLower()).SingleOrDefault();
          if (item != null)
          {
            if (item.SubText.Trim().ToLower() == newMenu.SubText.Trim().ToLower())
            {
              var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
          newMenu.Text.Trim().ToLower());
              UpdateMenuChangeType(ref updItems, ChangeType.Default);
            }
            else
            {
              var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
          newMenu.Text.Trim().ToLower());
              UpdateMenuChangeType(ref updItems, ChangeType.Edit);
            }
          }
          else
          {
            var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
           newMenu.Text.Trim().ToLower());
            UpdateMenuChangeType(ref updItems, ChangeType.New);
          }
        }
      }
      else if ((oldrequest.Menus != null && oldrequest.Menus.Count == 0 &&
        request.Menus != null && request.Menus.Count > 0) ||
          (oldrequest.Menus == null &&
        request.Menus != null && request.Menus.Count > 0))
      {
        request.Menus.ToList().ForEach(m => m.ChangeType = ChangeType.New);
      }

      if (oldrequest.Menus != null && oldrequest.Menus.Count > 0)
      {
        oldrequest.Menus = oldrequest.Menus.Where
          (m => m.ChangeType != ChangeType.Delete).ToList();

        var diff =
          oldrequest.Menus.Where(s => !request.Menus.Any
          (r => s.Text.Trim().ToLower() == r.Text.Trim().ToLower()));
        if (diff != null)
        {
          UpdateMenuChangeType(ref diff, ChangeType.Delete);
          request.Menus = request.Menus.Concat(diff.ToList()).ToList();
        }
      }
    }

    private static void UpdateMenuChangeType(ref IEnumerable<Model.Menu> updItems,
      ChangeType changeType)
    {
      if (updItems != null && updItems.Count() > 0)
      {
        foreach (var uItem in updItems)
        {
          uItem.ChangeType = changeType;
        }
      }
    }

    private void UpdateIngredientChangeFlags(Model.Request oldrequest, Model.Request request)
    {
      if (oldrequest.Ingredients != null && oldrequest.Ingredients.Count > 0 &&
  request.Ingredients != null && request.Ingredients.Count > 0)
      {
        oldrequest.Ingredients = oldrequest.Ingredients.Where
        (m => m.ChangeType != ChangeType.Delete).ToList();

        foreach (var newIng in request.Ingredients)
        {
          var item = oldrequest.Ingredients.Where(m => m.Text.Trim().ToLower() ==
          newIng.Text.Trim().ToLower()).SingleOrDefault();
          if (item != null)
          {
            if (item.SubText.Trim().ToLower() == newIng.SubText.Trim().ToLower())
            {
              var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
          newIng.Text.Trim().ToLower());
              UpdateIngredientChangeType(ref updItems, ChangeType.Default);
            }
            else
            {
              var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
          newIng.Text.Trim().ToLower());
              UpdateIngredientChangeType(ref updItems, ChangeType.Edit);
            }
          }
          else
          {
            var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
           newIng.Text.Trim().ToLower());
            UpdateIngredientChangeType(ref updItems, ChangeType.New);
          }
        }
      }
      else if ((oldrequest.Ingredients != null && oldrequest.Ingredients.Count == 0 &&
        request.Ingredients != null && request.Ingredients.Count > 0) ||
          (oldrequest.Menus == null &&
        request.Ingredients != null && request.Ingredients.Count > 0))
      {
        request.Ingredients.ToList().ForEach(m => m.ChangeType = ChangeType.New);
      }
      if (oldrequest.Ingredients != null && oldrequest.Ingredients.Count > 0)
      {
        oldrequest.Ingredients = oldrequest.Ingredients.Where
        (m => m.ChangeType != ChangeType.Delete).ToList();

        var diff =
          oldrequest.Ingredients.Where(s => !request.Ingredients.Any
          (r => s.Text.Trim().ToLower() == r.Text.Trim().ToLower()));
        if (diff != null)
        {
          UpdateIngredientChangeType(ref diff, ChangeType.Delete);
          request.Ingredients = request.Ingredients.Concat(diff.ToList()).ToList();
        }
      }
    }

    private static void UpdateIngredientChangeType(ref IEnumerable<Model.Ingredient> updItems,
     ChangeType changeType)
    {
      if (updItems != null && updItems.Count() > 0)
      {
        foreach (var uItem in updItems)
        {
          uItem.ChangeType = changeType;
        }
      }
    }

    private void UpdateHalalTeamChangeFlags(Model.Request oldrequest, Model.Request request)
    {
      if (oldrequest.Teams != null && oldrequest.Teams.Count > 0 &&
  request.Teams != null && request.Teams.Count > 0)
      {
        oldrequest.Teams = oldrequest.Teams.Where
          (m => m.ChangeType != ChangeType.Delete).ToList();

        foreach (var team in request.Teams)
        {
          var item = oldrequest.Teams.Where(m => m.AltID.Trim().ToLower() ==
          team.AltID.Trim().ToLower()).SingleOrDefault();
          if (item != null)
          {
            if (item.Name.Trim().ToLower() == team.Name.Trim().ToLower() &&
              item.IsCertified == team.IsCertified &&
              item.JoinedOn == team.JoinedOn &&
               item.Designation.Trim().ToLower() == team.Designation.Trim().ToLower())
            {
              var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
          team.AltID.Trim().ToLower());
              UpdateHalalTeamChangeType(ref updItems, ChangeType.Default);
            }
            else
            {
              var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
          team.AltID.Trim().ToLower());
              UpdateHalalTeamChangeType(ref updItems, ChangeType.Edit);
            }
          }
          else
          {
            var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
           team.AltID.Trim().ToLower());
            UpdateHalalTeamChangeType(ref updItems, ChangeType.New);
          }
        }
      }
      else if ((oldrequest.Teams != null && oldrequest.Teams.Count == 0 &&
        request.Teams != null && request.Teams.Count > 0) ||
          (oldrequest.Menus == null &&
        request.Teams != null && request.Teams.Count > 0))
      {
        request.Teams.ToList().ForEach(m => m.ChangeType = ChangeType.New);
      }
      if (oldrequest.Teams != null && oldrequest.Teams.Count > 0 && request.Teams != null)
      {
        oldrequest.Teams = oldrequest.Teams.Where
       (m => m.ChangeType != ChangeType.Delete).ToList();

        var diff =
            oldrequest.Teams.Where(s => !request.Teams.Any
            (r => s.AltID.Trim().ToLower() == r.AltID.Trim().ToLower()));
        if (diff != null)
        {
          UpdateHalalTeamChangeType(ref diff, ChangeType.Delete);
          request.Teams = request.Teams.Concat(diff.ToList()).ToList();
        }
      }
    }

    private static void UpdateHalalTeamChangeType(ref IEnumerable<HalalTeam> updItems,
   ChangeType changeType)
    {
      if (updItems != null && updItems.Count() > 0)
      {
        foreach (var uItem in updItems)
        {
          uItem.ChangeType = changeType;
        }
      }
    }

    private void UpdatePremiseChangeFlags(Model.Request oldrequest, Model.Request request)
    {
      if (oldrequest.Premises != null && oldrequest.Premises.Count > 0 &&
  request.Premises != null && request.Premises.Count > 0)
      {
        oldrequest.Premises = oldrequest.Premises.Where
          (m => m.ChangeType != ChangeType.Delete).ToList();

        foreach (var premise in request.Premises)
        {

          foreach (Premise prem in oldrequest.Premises)
          {
            if (string.IsNullOrEmpty(prem.FloorNo))
            {
              prem.FloorNo = "";
            }
            if (string.IsNullOrEmpty(prem.UnitNo))
            {
              prem.UnitNo = "";
            }
            if (string.IsNullOrEmpty(prem.Postal))
            {
              prem.Postal = "";
            }
            if (string.IsNullOrEmpty(prem.Address1))
            {
              prem.Address1 = "";
            }
            if (string.IsNullOrEmpty(prem.BuildingName))
            {
              prem.BuildingName = "";
            }
          }

          foreach (Premise prem in request.Premises)
          {
            if (string.IsNullOrEmpty(prem.FloorNo))
            {
              prem.FloorNo = "";
            }
            if (string.IsNullOrEmpty(prem.UnitNo))
            {
              prem.UnitNo = "";
            }
            if (string.IsNullOrEmpty(prem.Postal))
            {
              prem.Postal = "";
            }
            if (string.IsNullOrEmpty(prem.Address1))
            {
              prem.Address1 = "";
            }
            if (string.IsNullOrEmpty(prem.BuildingName))
            {
              prem.BuildingName = "";
            }
          }


          var item = oldrequest.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
          premise.FloorNo.Trim().ToLower() &&
          m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
          m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower()).SingleOrDefault();
          if (item != null)
          {
            if (item.Address1.Trim().ToLower() == premise.Address1.Trim().ToLower() &&
              item.BuildingName.Trim().ToLower() == premise.BuildingName.Trim().ToLower())
            {
              var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
          premise.FloorNo.Trim().ToLower() &&
          m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
          m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
              UpdatePremiseChangeType(ref updItems, ChangeType.Default);
            }
            else
            {
              var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
          premise.FloorNo.Trim().ToLower() &&
          m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
          m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
              UpdatePremiseChangeType(ref updItems, ChangeType.Edit);
            }
          }
          else
          {
            var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
          premise.FloorNo.Trim().ToLower() &&
          m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
          m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
            UpdatePremiseChangeType(ref updItems, ChangeType.New);
          }
        }
      }
      else if ((oldrequest.Premises != null && oldrequest.Premises.Count == 0 &&
        request.Premises != null && request.Premises.Count > 0) ||
          (oldrequest.Premises == null &&
        request.Premises != null && request.Premises.Count > 0))
      {
        request.Premises.ToList().ForEach(m => m.ChangeType = ChangeType.New);
      }
      if (oldrequest.Premises != null && oldrequest.Premises.Count > 0)
      {
        oldrequest.Premises = oldrequest.Premises.Where
       (m => m.ChangeType != ChangeType.Delete).ToList();

        var diff =
            oldrequest.Premises.Where(s => !request.Premises.Any
            (r => s.FloorNo.Trim().ToLower() == r.FloorNo.Trim().ToLower() &&
            s.UnitNo.Trim().ToLower() == r.UnitNo.Trim().ToLower() &&
            s.Postal.Trim().ToLower() == r.Postal.Trim().ToLower()));
        if (diff != null)
        {
          UpdatePremiseChangeType(ref diff, ChangeType.Delete);
          request.Premises = request.Premises.Concat(diff.ToList()).ToList();
        }
      }
    }

    private static void UpdatePremiseChangeType(ref IEnumerable<Premise> updItems,
   ChangeType changeType)
    {
      if (updItems != null && updItems.Count() > 0)
      {
        foreach (var uItem in updItems)
        {
          uItem.ChangeType = changeType;
        }
      }
    }

    private Characteristic CharacteristicWithCertificate(
    IList<Characteristic> characteristics)
    {
      foreach (Characteristic ch in characteristics)
      {
        if (ch.Type == RequestCharType.IssuedCertificate)
          return ch;
      }

      return null;
    }

    //private void UpdateIngredientChangeFlags(
    //  Model.Request oldrequest,
    //  Model.Request request)
    //{
    //  if (oldrequest.Ingredients != null)
    //  {
    //    if (request.Ingredients != null)
    //    {
    //      bool newingredientchanged = false;
    //      foreach (Ingredient oldingredient in oldrequest.Ingredients)
    //      {
    //        if (oldingredient.ChangeType == ChangeType.Delete)
    //          continue;

    //        foreach (Ingredient newingredient in request.Ingredients)
    //        {
    //          if ((newingredient.Text == oldingredient.Text) &&
    //            (newingredient.SubText == oldingredient.SubText) &&
    //            (newingredient.Approved == oldingredient.Approved) &&
    //            (newingredient.Remarks == oldingredient.Remarks) &&
    //            (newingredient.RiskCategory == oldingredient.RiskCategory))

    //          {
    //            newingredient.ChangeType = ChangeType.Default;
    //            newingredientchanged = true;
    //          }
    //          else if ((newingredient.Text == oldingredient.Text) &&
    //            (newingredient.SubText != oldingredient.SubText ||
    //            newingredient.Approved != oldingredient.Approved ||
    //            newingredient.Remarks != oldingredient.Remarks) ||
    //            newingredient.RiskCategory != oldingredient.RiskCategory)
    //          {
    //            newingredient.ChangeType = ChangeType.Edit;
    //            newingredientchanged = true;
    //          }
    //          else if (newingredient.Text != oldingredient.Text)
    //          {
    //            newingredient.ChangeType = ChangeType.New;
    //            newingredientchanged = true;
    //          }
    //        }
    //        if (newingredientchanged == false
    //          && oldingredient.ChangeType != ChangeType.Delete)
    //        {
    //          Ingredient newingredientrecord = new Ingredient();
    //          newingredientrecord = oldingredient;
    //          newingredientrecord.ChangeType = ChangeType.Delete;
    //          request.Ingredients.Add(newingredientrecord);
    //        }


    //      }
    //    }

    //    if (request.Ingredients == null)
    //    {
    //      request.Ingredients = new List<Ingredient>();
    //      foreach (Ingredient oldingredient in oldrequest.Ingredients)
    //      {
    //        if (oldingredient.ChangeType != ChangeType.Delete)
    //        {
    //          Ingredient newingredientrecord = new Ingredient();
    //          newingredientrecord = oldingredient;
    //          newingredientrecord.ChangeType = ChangeType.Delete;
    //          request.Ingredients.Add(newingredientrecord);

    //        }
    //      }
    //    }
    //  }
    //  else if (request.Ingredients != null)
    //  {
    //    foreach (Ingredient newingredient in request.Ingredients)
    //      newingredient.ChangeType = ChangeType.New;
    //  }
    //}

    //private void UpdateHalalTeamChangeFlags(
    //  Model.Request oldrequest,
    //  Model.Request request)
    //{
    //  if (oldrequest.Teams != null)
    //  {
    //    if (request.Teams != null)
    //    {
    //      bool newteamchanged = false;
    //      foreach (HalalTeam oldteam in oldrequest.Teams)
    //      {
    //        if (oldteam.ChangeType == ChangeType.Delete)
    //          continue;

    //        foreach (HalalTeam newteam in request.Teams)
    //        {
    //          if ((newteam.Name == oldteam.Name) &&
    //            (newteam.AltID == oldteam.AltID) &&
    //            (newteam.Designation == oldteam.Designation) &&
    //            (newteam.Role == oldteam.Role) &&
    //            (newteam.IsCertified == oldteam.IsCertified))
    //          {
    //            newteam.ChangeType = ChangeType.Default;
    //            newteamchanged = true;
    //          }
    //          else if ((newteam.Name == oldteam.Name) &&
    //            (newteam.AltID != oldteam.AltID ||
    //            newteam.Designation != oldteam.Designation ||
    //            newteam.Role != oldteam.Role ||
    //            newteam.IsCertified != oldteam.IsCertified))
    //          {
    //            newteam.ChangeType = ChangeType.Edit;
    //            newteamchanged = true;
    //          }
    //          else if (newteam.Name != oldteam.Name)
    //          {
    //            newteam.ChangeType = ChangeType.New;
    //            newteamchanged = true;
    //          }
    //        }

    //        if (newteamchanged == false && oldteam.ChangeType != ChangeType.Delete)
    //        {
    //          HalalTeam newteamrecord = new HalalTeam();
    //          newteamrecord = oldteam;
    //          newteamrecord.ChangeType = ChangeType.Delete;
    //          request.Teams.Add(newteamrecord);
    //        }


    //      }

    //    }

    //    if (request.Teams == null)
    //    {
    //      foreach (HalalTeam oldteam in oldrequest.Teams)
    //      {
    //        request.Teams = new List<HalalTeam>();
    //        if (oldteam.ChangeType != ChangeType.Delete)
    //        {
    //          HalalTeam newteamrecord = new HalalTeam();
    //          newteamrecord = oldteam;
    //          newteamrecord.ChangeType = ChangeType.Delete;
    //          request.Teams.Add(newteamrecord);
    //        }

    //      }
    //    }
    //  }
    //  else if (request.Teams != null)
    //  {
    //    foreach (HalalTeam newteam in request.Teams)
    //      newteam.ChangeType = ChangeType.New;
    //  }

    //}

    //private void UpdatePremiseChangeFlags(
    //  Model.Request oldrequest,
    //  Model.Request request)
    //{
    //  if (oldrequest.Premises != null)
    //  {
    //    if (request.Premises != null)
    //    {
    //      bool newpremisechanged = false;
    //      foreach (Premise oldpremise in oldrequest.Premises)
    //      {
    //        if (oldpremise.ChangeType == ChangeType.Delete)
    //          continue;

    //        foreach (Premise newpremise in request.Premises)
    //        {
    //          if ((newpremise.Name == oldpremise.Name) &&
    //            (newpremise.BlockNo == oldpremise.BlockNo) &&
    //            (newpremise.Area == oldpremise.Area) &&
    //            (newpremise.City == oldpremise.City) &&
    //            (newpremise.Country == oldpremise.Country) &&
    //            (newpremise.FloorNo == oldpremise.FloorNo) &&
    //            (newpremise.Postal == oldpremise.Postal) &&
    //            (newpremise.Province == oldpremise.Province))
    //          {
    //            newpremise.ChangeType = ChangeType.Default;
    //            newpremisechanged = true;
    //          }
    //          else if ((newpremise.Name == oldpremise.Name) &&
    //            (newpremise.BlockNo != oldpremise.BlockNo ||
    //            newpremise.Area != oldpremise.Area ||
    //            newpremise.City != oldpremise.City ||
    //            newpremise.Country != oldpremise.Country ||
    //            newpremise.FloorNo != oldpremise.FloorNo ||
    //            newpremise.Postal != oldpremise.Postal ||
    //            newpremise.Province != oldpremise.Province))
    //          {
    //            newpremise.ChangeType = ChangeType.Edit;
    //            newpremisechanged = true;
    //          }
    //          else if (newpremise.Name != oldpremise.Name)
    //          {
    //            newpremise.ChangeType = ChangeType.New;
    //            newpremisechanged = true;
    //          }
    //        }
    //        if (newpremisechanged == false
    //          && oldpremise.ChangeType != ChangeType.Delete)
    //        {
    //          Premise newpremiserecord = new Premise();
    //          newpremiserecord = oldpremise;
    //          newpremiserecord.ChangeType = ChangeType.Delete;
    //          request.Premises.Add(newpremiserecord);
    //        }


    //      }
    //    }

    //    if (request.Premises == null)
    //    {
    //      request.Premises = new List<Premise>();
    //      foreach (Premise oldpremise in oldrequest.Premises)
    //      {
    //        if (oldpremise.ChangeType != ChangeType.Delete)
    //        {
    //          Premise newpremiserecord = new Premise();
    //          newpremiserecord = oldpremise;
    //          newpremiserecord.ChangeType = ChangeType.Delete;
    //          request.Premises.Add(newpremiserecord);

    //        }
    //      }
    //    }
    //  }
    //  else if (request.Premises != null)
    //  {
    //    foreach (Premise newpremise in request.Premises)
    //      newpremise.ChangeType = ChangeType.New;
    //  }
    //}
  }
}

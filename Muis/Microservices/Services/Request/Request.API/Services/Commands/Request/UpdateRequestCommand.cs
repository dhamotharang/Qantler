using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Request.API.Services.Commands.Request
{
  public class UpdateRequestCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly Model.Request _request;

    public UpdateRequestCommand(Model.Request request)
    {
      _request = request;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      Model.Request request = null;

      Model.Request exisingRequest = await dbContext.Request.GetRequestByID(_request.ID);
      UpdateChangeFlags(exisingRequest, _request);

      if (exisingRequest != null)
      {
        if (_request != null && _request.LineItems != null && _request.LineItems.Count > 0)
        {
          foreach (var item in _request.LineItems)
          {
            var lineItemID = await dbContext.Request.UpdateRequestLineItem(item, _request.ID);
            item.ID = lineItemID;
            if (item.Characteristics != null && item.Characteristics.Count > 0)
            {
              foreach (var chars in item.Characteristics)
              {
                await dbContext.Request.UpdateRequestLineItemCharacteristics(chars, item.ID);
              }
            }
          }
          if (_request.Characteristics != null && _request.Characteristics.Count > 0)
          {
            foreach (var reqChars in _request.Characteristics)
            {
              await dbContext.Request.UpdateRequestCharacteristics(reqChars, _request.ID);
            }
          }
          if (_request.Attachments != null && _request.Attachments.Count > 0)
          {
            foreach (var attach in _request.Attachments)
            {
              var attachmentID = await dbContext.Request.InsertRequestAttachments
                (attach, _request.ID);
              attach.ID = attachmentID;
            }
          }
          if (_request.Premises != null && _request.Premises.Count > 0)
          {
            foreach (var prem in _request.Premises)
            {
              await dbContext.Request.UpdateRequestPremise(prem, _request.ID);
            }
          }
          if (_request.Teams != null && _request.Teams.Count > 0)
          {
            foreach (var team in _request.Teams)
            {
              await dbContext.Request.UpdateRequestHalalTeam(team, _request.ID);
            }
          }
          if (_request.Menus != null && _request.Menus.Count > 0)
          {
            foreach (var mens in _request.Menus)
            {
              await dbContext.Request.UpdateRequestMenu(mens, _request.ID);
            }
          }
          if (_request.Ingredients != null && _request.Ingredients.Count > 0)
          {
            foreach (var ing in _request.Ingredients)
            {
              await dbContext.Request.UpdateRequestIngredient(ing, _request.ID);
            }
          }
        }

        request = await dbContext.Request.GetRequestByID(_request.ID);
      }

      unitOfWork.Commit();

      return request;
    }

    private void UpdateChangeFlags(Model.Request oldrequest, Model.Request request)
    {
      UpdateRequestLineItemAndChars(oldrequest, request);
      UpdateMenusChangeFlags(oldrequest, request);
      UpdateIngredientChangeFlags(oldrequest, request);
      UpdatePremiseChangeFlags(oldrequest, request);
      UpdateHalalTeamChangeFlags(oldrequest, request);
    }



    private void UpdateRequestLineItemAndChars(Model.Request oldrequest, Model.Request request)
    {
      if (oldrequest.LineItems != null && oldrequest.LineItems.Count > 0)
      {
        if (request.LineItems != null && request.LineItems.Count > 0)
        {
          foreach (var lineItem in request.LineItems)
          {
            var result = oldrequest.LineItems.Where(l => l.Scheme == lineItem.Scheme
            && l.SubScheme == lineItem.SubScheme).FirstOrDefault();
            if (result != null)
            {
              lineItem.IsDeleted = false;
              lineItem.RequestID = result.RequestID;
              lineItem.ID = result.ID;
              if (lineItem.Characteristics != null && lineItem.Characteristics.Count > 0)
              {
                foreach (var chars in lineItem.Characteristics)
                {
                  var charResult = result.Characteristics.Where(l => l.Type == chars.Type
              && l.Value == chars.Value).FirstOrDefault();
                  if (charResult != null)
                  {
                    chars.IsDeleted = false;
                    chars.ID = charResult.ID;
                  }
                }
              }
            }

          }
          foreach (var lineItem in oldrequest.LineItems)
          {
            var result = request.LineItems.Where(l => l.Scheme == lineItem.Scheme
            && l.SubScheme == lineItem.SubScheme).FirstOrDefault();
            if (result == null)
            {
              lineItem.IsDeleted = true;
              lineItem.RequestID = result.RequestID;
              lineItem.ID = result.ID;
              if (lineItem.Characteristics != null && lineItem.Characteristics.Count > 0)
              {
                foreach (var chars in lineItem.Characteristics)
                {
                  var charResult = result.Characteristics.Where(l => l.Type == chars.Type
              && l.Value == chars.Value).FirstOrDefault();
                  if (charResult != null)
                  {
                    chars.IsDeleted = true;
                    chars.ID = charResult.ID;
                  }
                }
              }
            }
          }
        }
      }
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
          newMenu.Text.Trim().ToLower()).FirstOrDefault();
          if (item != null)
          {
            if (item.SubText.Trim().ToLower() == newMenu.SubText.Trim().ToLower())
            {
              //    var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
              //newMenu.Text.Trim().ToLower());
              //    UpdateMenuChangeType(ref updItems, ChangeType.Default);
              newMenu.ChangeType = ChangeType.Default;
              newMenu.ID = item.ID;
              newMenu.RequestID = oldrequest.ID;
            }
            else
            {
              //    var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
              //newMenu.Text.Trim().ToLower());
              //    UpdateMenuChangeType(ref updItems, ChangeType.Edit);
              newMenu.ChangeType = ChangeType.Edit;
              newMenu.ID = item.ID;
              newMenu.RequestID = oldrequest.ID;
            }
          }
          else
          {
            // var updItems = request.Menus.Where(m => m.Text.Trim().ToLower() ==
            //newMenu.Text.Trim().ToLower());
            // UpdateMenuChangeType(ref updItems, ChangeType.New);
            newMenu.ChangeType = ChangeType.New;
            newMenu.RequestID = oldrequest.ID;
          }
        }
      }
      else if (oldrequest.Menus == null && request.Menus != null && request.Menus.Count > 0)
      {
        request.Menus.ToList().ForEach(m => m.ChangeType = ChangeType.New);
        request.Menus.ToList().ForEach(m => m.RequestID = oldrequest.ID);
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
              //    var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
              //newIng.Text.Trim().ToLower());
              //    UpdateIngredientChangeType(ref updItems, ChangeType.Default);
              newIng.ChangeType = ChangeType.Default;
              newIng.ID = item.ID;
              newIng.RequestID = oldrequest.ID;
            }
            else
            {
              //    var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
              //newIng.Text.Trim().ToLower());
              //    UpdateIngredientChangeType(ref updItems, ChangeType.Edit);
              newIng.ChangeType = ChangeType.Edit;
              newIng.ID = item.ID;
              newIng.RequestID = oldrequest.ID;
            }
          }
          else
          {
            // var updItems = request.Ingredients.Where(m => m.Text.Trim().ToLower() ==
            //newIng.Text.Trim().ToLower());
            // UpdateIngredientChangeType(ref updItems, ChangeType.New);
            newIng.ChangeType = ChangeType.New;
            newIng.RequestID = oldrequest.ID;
          }
        }
      }
      else if (oldrequest.Menus == null && request.Ingredients != null && request.Ingredients.Count > 0)
      {
        request.Ingredients.ToList().ForEach(m => m.ChangeType = ChangeType.New);
        request.Ingredients.ToList().ForEach(m => m.RequestID = oldrequest.ID);
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
              //    var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
              //team.AltID.Trim().ToLower());
              //    UpdateHalalTeamChangeType(ref updItems, ChangeType.Default);
              team.ChangeType = ChangeType.Default;
              team.ID = item.ID;
              team.RequestID = oldrequest.ID;
            }
            else
            {
              //    var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
              //team.AltID.Trim().ToLower());
              //    UpdateHalalTeamChangeType(ref updItems, ChangeType.Edit);
              team.ChangeType = ChangeType.Edit;
              team.ID = item.ID;
              team.RequestID = oldrequest.ID;
            }
          }
          else
          {
            // var updItems = request.Teams.Where(m => m.AltID.Trim().ToLower() ==
            //team.AltID.Trim().ToLower());
            // UpdateHalalTeamChangeType(ref updItems, ChangeType.New);
            team.ChangeType = ChangeType.New;
            team.RequestID = oldrequest.ID;
          }
        }
      }
      else if (oldrequest.Teams == null && request.Teams != null && request.Teams.Count > 0)
      {
        request.Teams.ToList().ForEach(m => m.ChangeType = ChangeType.New);
        request.Teams.ToList().ForEach(m => m.RequestID = oldrequest.ID);
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

        foreach (var premise in request.Premises)
        {
          var item = oldrequest.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
          premise.FloorNo.Trim().ToLower() &&
          m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
          m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower()).SingleOrDefault();
          if (item != null)
          {
            if (item.Address1.Trim().ToLower() == premise.Address1.Trim().ToLower() &&
              item.BuildingName.Trim().ToLower() == premise.BuildingName.Trim().ToLower())
            {
              //    var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
              //premise.FloorNo.Trim().ToLower() &&
              //m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
              //m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
              //    UpdatePremiseChangeType(ref updItems, ChangeType.Default);
              premise.ChangeType = ChangeType.Default;
              premise.ID = item.ID;
              premise.RequestID = oldrequest.ID;
            }
            else
            {
              //    var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
              //premise.FloorNo.Trim().ToLower() &&
              //m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
              //m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
              //    UpdatePremiseChangeType(ref updItems, ChangeType.Edit);
              premise.ChangeType = ChangeType.Edit;
              premise.ID = item.ID;
              premise.RequestID = oldrequest.ID;
            }
          }
          else
          {
            //  var updItems = request.Premises.Where(m => m.FloorNo.Trim().ToLower() ==
            //premise.FloorNo.Trim().ToLower() &&
            //m.UnitNo.Trim().ToLower() == premise.UnitNo.Trim().ToLower() &&
            //m.Postal.Trim().ToLower() == premise.Postal.Trim().ToLower());
            //  UpdatePremiseChangeType(ref updItems, ChangeType.New);
            premise.ChangeType = ChangeType.Edit;
            premise.RequestID = oldrequest.ID;
          }
        }
      }
      else if (oldrequest.Premises == null && request.Premises != null && request.Premises.Count > 0)
      {
        request.Premises.ToList().ForEach(m => m.ChangeType = ChangeType.New);
        request.Premises.ToList().ForEach(m => m.RequestID = oldrequest.ID);
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

  }
}

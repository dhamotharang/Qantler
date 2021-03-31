using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;


namespace Request.API.Services.Commands.Request
{
  public class ProceedForReviewCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;
    readonly Officer _officer;
    readonly IEventBus _eventBus;

    public ProceedForReviewCommand(long requestID, Officer officer, IEventBus eventBus)
    {
      _requestID = requestID;
      _officer = officer;
      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var request = await dbContext.Request.GetRequestByID(_requestID);
      if (request == null)
      {
        throw new BadRequestException(
         await dbContext.Transalation.GetTranslation(Locale.EN, "RequestNotFound"));
      }

      var customer = await dbContext.Customer.GetByID(request.CustomerID);
      if (customer.CodeID == null)
      {
        throw new BadRequestException(
          await dbContext.Transalation.GetTranslation(Locale.EN, "RequestReviewNoCode"));
      }

      await dbContext.Request.UpdateStatus(_requestID, RequestStatus.Open, null);

      if (_officer != null && _officer.Name != null)
      {
        await dbContext.Request.SetAssignedOfficer(_requestID, _officer.ID, _officer.Name);

        await SendNotificationToAssignedOfficer(dbContext, _officer);
      }
      else
      {
        var premise = request.Premises?.FirstOrDefault(e => e.IsPrimary);
        if (premise != null)
        {
          // Send Push Notification to admin
          await SendNotificationToAdmin(dbContext, premise.Postal, request.RefID);
        }
      }

      unitOfWork.Commit();

      return Unit.Default;
    }

    async Task SendNotificationToAdmin(DbContext dbContext,
      string postalCode, string refID)
    {
      _eventBus.Publish(new SendNotificationWithPermissionsEvent
      {
        Title = await dbContext.Transalation.GetTranslation(
          Locale.EN, "AssignCANotFoundNotifTitle"),
        Body = string.Format(await dbContext.Transalation.GetTranslation(
          Locale.EN, "AssignCANotFoundNotifBody"),
          postalCode),
        Module = "Request",
        RefID = $"{refID}",
        Permissions = new Permission[] {
          Permission.RequestReassign,
          Permission.RequestOverride
        }
      });
    }

    async Task SendNotificationToAssignedOfficer(DbContext dbContext, Officer user)
    {
      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationTitle"),
        Body = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationBody"),
        Module = "Request",
        To = new List<string> { user.ID.ToString() }
      });
    }
  }
}

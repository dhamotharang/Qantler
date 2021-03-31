using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class OnBillGeneratedForRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;

    readonly IEventBus _eventBus;

    public OnBillGeneratedForRequestCommand(long requestID, IEventBus eventBus)
    {
      _requestID = requestID;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      if (_requestID > 0L)
      {
        var dbContext = DbContext.From(unitOfWork);
        unitOfWork.BeginTransaction();

        var request = await dbContext.Request.GetRequestByIDBasic(_requestID);
        if (request != null
          && request.StatusMinor != RequestStatusMinor.BillReady)
        {
          await dbContext.Request.UpdateStatus(_requestID,
            request.Status,
            RequestStatusMinor.BillReady);

          if (request.Status == RequestStatus.PendingBill)
          {
            _eventBus.Publish(new SendNotificationWithPermissionsEvent
            {
              Title = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestBillReadyTitle"),
              Body = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestBillReadyBody"),
              Module = "Request",
              RefID = $"{_requestID}",
              Permissions = new List<Permission> { Permission.RequestInvoice }
            });
          }
        }

        unitOfWork.Commit();
      }
      return Unit.Default;
    }
  }
}

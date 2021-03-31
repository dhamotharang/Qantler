using System;
using System.Collections.Generic;
using System.Linq;
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
  public class OnBillPaidCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;

    readonly IEventBus _eventBus;

    public OnBillPaidCommand(long requestID, IEventBus eventBus)
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

        if (   request != null
            && request.Status == RequestStatus.PendingPayment)
        {
          var certificate = await dbContext.Certificate.GetByRequestID(_requestID);

          var status = certificate.Status == CertificateDeliveryStatus.Delivered
            ? RequestStatus.Closed
            : RequestStatus.Issuance;

          await dbContext.Request.UpdateStatus(_requestID, status, null);

          _eventBus.Publish(new SendNotificationWithPermissionsEvent
          {
            Title = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestBillPaidTitle"),
            Body = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestBillPaidBody"),
            Module = "Request",
            RefID = $"{_requestID}",
            Permissions = new List<Permission> { Permission.RequestInvoice }
          });
        }

        unitOfWork.Commit();
      }
      return Unit.Default;
    }
  }
}

using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Helpers;
using Request.API.Repository;
using Request.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Cert
{
  public class AcknowledgeCertificateBatchCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long[] _ids;
    readonly Officer _user;

    readonly IEventBus _eventBus;

    public AcknowledgeCertificateBatchCommand(long[] ids, Officer user, IEventBus eventBus)
    {
      _ids = ids;
      _user = user;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      foreach (long id in _ids)
      {
        await dbContext.Certificate.ExecAcknowledgeBatch(id, _user);

        var batch = await dbContext.Certificate.GetCertificateBatchByID(id);

        var msg = string.Format(
          await dbContext.Transalation.GetTranslation(Locale.EN, "MuftiAcknowledgeNotifBody"),
          batch.Code);

        foreach (var certificate in batch.Certificates)
        {
          var request = await dbContext.Request.GetRequestByIDBasic(certificate.RequestID);

          var approvedLineItems = request?.LineItems;

          var stage2Items = approvedLineItems.Where(e => RequestHelper.HasStage2Payment(
            request.Type,
            e.Scheme,
            e.SubScheme)).ToList();

          if (stage2Items.Any())
          {
            if (request.StatusMinor == Model.RequestStatusMinor.BillReady)
            {
              await dbContext.Request.UpdateStatus(request.ID, Model.RequestStatus.PendingBill,
                statusMinor: Model.RequestStatusMinor.BillReady);

              _eventBus.Publish(new SendNotificationWithPermissionsEvent
              {
                Title = await dbContext.Transalation.GetTranslation(Locale.EN,
                "RequestBillReadyTitle"),
                Body = await dbContext.Transalation.GetTranslation(Locale.EN,
                "RequestBillReadyBody"),
                Module = "Request",
                RefID = $"{request.ID}",
                Permissions = new List<Permission> { Permission.RequestInvoice }
              });
            }
          }
          else
          {
            await dbContext.Request.UpdateStatus(request.ID, Model.RequestStatus.Issuance, null);
          }
        }

        _eventBus.Publish(new SendNotificationWithPermissionsEvent
        {
          Title = await dbContext.Transalation.GetTranslation(Locale.EN,
          "MuftiAcknowledgeNotifTitle"),
          Body = msg,
          Module = "CertificateBatch",
          RefID = $"{id}",
          Permissions = new List<Permission> { Permission.RequestIssuance },
          Excludes = new string[] { _user.ID.ToString() }
        });
      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

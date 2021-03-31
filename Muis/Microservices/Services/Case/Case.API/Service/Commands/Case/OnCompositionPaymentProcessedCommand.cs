using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class OnCompositionPaymentProcessedCommand : IUnitOfWorkCommand<long>
  {
    readonly PaymentStatus _paymentStatus;
    readonly long _caseID;
    readonly IEventBus _eventBus;

    public OnCompositionPaymentProcessedCommand(long caseID,
      PaymentStatus paymentStatus, IEventBus eventBus)
    {
      _paymentStatus = paymentStatus;
      _caseID = caseID;
      _eventBus = eventBus;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      var action = "";

      if (_paymentStatus == PaymentStatus.Processed)
      {
        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed);

        action = await dbContext.Translation.GetTranslation(Locale.EN, "PaymentProcessed");
      }
      else if (_paymentStatus == PaymentStatus.Rejected)
      {
        await dbContext.Case.UpdateStatus(_caseID, @case.Status,
          CaseMinorStatus.PaymentRejected);

        action = await dbContext.Translation.GetTranslation(Locale.EN, "PaymentRejected");
      }

      await dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.Payment,
        Action = action
      }, _caseID);


      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Translation.GetTranslation(Locale.EN, "InviteCompositionPaymentTitle"),
        Body = action,
        Module = "Case",
        RefID = $"{_caseID}",
        To = new string[] { @case.ManagedByID.ToString() }
      });

      unitOfWork.Commit();
      return _caseID;
    }
  }
}

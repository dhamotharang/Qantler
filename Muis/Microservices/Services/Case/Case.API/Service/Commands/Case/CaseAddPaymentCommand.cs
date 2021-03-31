using Case.API.Params;
using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseAddPaymentCommand : IUnitOfWorkCommand<long>
  {
    readonly PaymentForComposition _payment;
    readonly Officer _user;
    readonly long _caseID;

    public CaseAddPaymentCommand(long caseID, PaymentForComposition payment, Officer user)
    {
      _payment = payment;
      _user = user;
      _caseID = caseID;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      await dbContext.Case.UpdateStatus(_caseID, @case.Status, CaseMinorStatus.PaymentReceived);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "AddPayment");

      var note = await dbContext.Translation.GetTranslation(Locale.EN, "AddPaymentNote");

      var activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.Payment,
        Action = action,
        User = _user,
        Notes = string.Format(note, _payment.BankAccountName,
        _payment.Amount, _payment.Notes),
        RefID = _payment.RefID,
      }, _caseID);

      unitOfWork.Commit();
      return activityID;
    }
  }
}

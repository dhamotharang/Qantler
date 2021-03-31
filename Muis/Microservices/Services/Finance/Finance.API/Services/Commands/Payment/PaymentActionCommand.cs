using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Finance.API.Repository;
using Finance.Events;
using Finance.Model;

namespace Finance.API.Services.Commands.Payment
{
  public class PaymentActionCommand : IUnitOfWorkCommand<Model.Payment>
  {
    readonly long _id;
    readonly PaymentStatus _status;
    readonly Officer _officer;
    readonly Model.Bank _bank;

    readonly IEventBus _eventBus;

    public PaymentActionCommand(long id, PaymentStatus status, Model.Bank bank, Officer officer,
      IEventBus eventBus)
    {
      _id = id;
      _status = status;
      _officer = officer;
      _bank = bank;

      _eventBus = eventBus;
    }

    public async Task<Model.Payment> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      uow.BeginTransaction();

      await dbContext.Payment.ExecPaymentAction(_id, _status, _officer);

      var result = await dbContext.Payment.GetPaymentByID(_id);

      if (result.Status == PaymentStatus.Processed)
      {
        foreach (var bill in result.Bills)
        {
          bill.Status = BillStatus.Paid;

          await dbContext.Bill.UpdateBillStatus(bill.ID, BillStatus.Paid);
        }
      }

      var logText = await dbContext.Translation.GetTranslation(Locale.EN,
        (result.Status == PaymentStatus.Processed ? "ApprovedPayment" : "RejectedPayment"));

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.Default,
        Action = logText,
        UserID = _officer.ID,
        UserName = _officer.Name
      });

      await dbContext.Payment.MapLog(_id, logID);

      if (result.Mode == PaymentMode.Offline
        && result.Status == PaymentStatus.Processed)
      {
        long bankID;

        var banks = await dbContext.Bank.Query(new BankFilter
        {
          AccountName= _bank.AccountName,
          AccountNo= _bank.AccountNo,
          BranchCode = _bank.BranchCode,
          BankName = _bank.BankName
        });

        if ((banks?.Count() ?? 0) == 1)
        {
          var bank = banks.First();

          bankID = bank.ID;

          await dbContext.Payment.DeleteMapBank(result.ID, bankID);

          if (bank.DDAStatus != DDAStatus.Approved)
          {
            bank.DDAStatus = DDAStatus.Approved;

            await dbContext.Bank.Update(bank);
          }
        }
        else
        {
          _bank.DDAStatus = DDAStatus.Approved;

          bankID = await dbContext.Bank.Insert(_bank);
        }
        await dbContext.Payment.MapBank(result.ID, bankID);
      }

      PublishEvents(result);

      uow.Commit();

      return result;
    }

    void PublishEvents(Model.Payment payment)
    {
      _eventBus.Publish(new OnPaymentProcessedEvent
      {
        ID = payment.ID,
        AltID = payment.AltID,
        RefNo = payment.RefNo,
        AccountID = payment.AccountID,
        Status = payment.Status,
        Bills = payment.Bills?.Select(e => new Events.Bill
        {
          ID = e.ID,
          Type = e.Type,
          RefID = e.RefID
        })?.ToList()
      });

      if (payment.Status == PaymentStatus.Processed)
      {
        foreach (var bill in payment.Bills)
        {
          _eventBus.Publish(new OnBillPaidEvent
          {
            ID = bill.ID,
            RefID = bill.RefID,
            Status = bill.Status,
            Type = bill.Type,
            RequestID = bill.RequestID,
            RefNo = bill.RefNo
          });
        }
      }
    }
  }
}

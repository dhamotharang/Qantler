using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Finance.API.DTO;
using Finance.API.Helpers;
using Finance.API.Repository;
using Finance.Events;
using Finance.Model;

namespace Finance.API.Services.Commands.Payment
{
  public class PaymentForCompositionCommand : IUnitOfWorkCommand<Model.Payment>
  {
    readonly IEventBus _eventBus;
    readonly PaymentForComposition _param;

    public PaymentForCompositionCommand(PaymentForComposition param, IEventBus eventBus)
    {
      _eventBus = eventBus;
      _param = param;
    }

    public async Task<Model.Payment> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      uow.BeginTransaction();

      var result = await GenerateBillPayment(dbContext);

      await InsertNotes(dbContext, result.Item2.ID);

      await SendNotification(dbContext, result.Item2.ID);

      uow.Commit();

      return result.Item2;
    }

    async Task<(Model.Bill, Model.Payment)> GenerateBillPayment(DbContext dbContext)
    {
      var gst = await FinanceHelper.GetGST(dbContext);
      var gstAmount = _param.Amount * gst;
      var amount = _param.Amount - gstAmount;

      var code = await dbContext.Transactioncode.GetTransactionCodeByCode(
        TransactionCodes.CompositionSum);
      if (code == null)
      {
        throw new BadRequestException("Composition sum code not setup. Please contact administrator.");
      }

      await dbContext.Account.InsertOrReplace(new Account
      {
        ID = _param.PayerID,
        Name = _param.PayerName
      });

      // Insert/replace contact peron
      await PersonHelper.InsertOrReplace(dbContext, _param.ContactPerson);

      var bill = new Model.Bill
      {
        Status = BillStatus.Pending,
        Type = BillType.CompositionSum,
        RefNo = _param.RefID,
        Amount = amount,
        GSTAmount = gstAmount,
        GST = gst,
        RefID = _param.RefID,
        CustomerID = _param.PayerID,
        CustomerName = _param.PayerName,
        IssuedOn = DateTime.UtcNow,
        LineItems = new List<BillLineItem>
        {
          new BillLineItem
          {
            SectionIndex = 0,
            Section = "Item Details",
            Index = 1,
            CodeID = code.ID,
            Descr = code.Text,
            Amount = amount,
            GSTAmount = gstAmount,
            GST = gst,
            WillRecord = true
          }
        }
      };

      bill.ID = await dbContext.Bill.InsertBill(bill);

      var payment = new Model.Payment
      {
        AltID = _param.BankAccountName,
        RefNo = _param.RefNo,
        Status = PaymentStatus.Pending,
        Mode = _param.Mode,
        Method = _param.Method,
        TransactionNo = Guid.NewGuid().ToString(),
        AccountID = _param.PayerID,
        Name = _param.PayerName,
        Amount = amount,
        GSTAmount = gstAmount,
        GST = gst,
        PaidOn = DateTime.UtcNow,
        ContactPersonID = _param.ContactPerson?.ID ?? null,
        ContactPerson = _param.ContactPerson,
        Bills = new List<Model.Bill> { bill }
      };

      payment.ID = await dbContext.Payment.Insert(payment);

      await dbContext.Payment.MapBill(payment.ID, bill.ID);

      if (_param.Mode == PaymentMode.Offline)
      {
        Model.Bank bank = null;
       
        var banks = await dbContext.Bank.Query(
          new BankFilter
          {
            AccountName= _param.BankAccountName
          });

        if ((banks?.Count() ?? 0) == 1)
        {
          bank = banks.First();

          await dbContext.Payment.MapBank(payment.ID, bank.ID);
        }
      }

      return (bill, payment);
    }

    async Task InsertNotes(DbContext dbContext, long paymentID)
    {
      var noteID = await dbContext.Note.InsertNotes(new Note
      {
        Text = _param.Notes,
        Officer = _param.Officer
      });

      if (_param.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _param.Attachments)
        {
          var attachmentID = await dbContext.Attachment.InsertAttachment(attachment);

          await dbContext.Note.MapAttachment(noteID, attachmentID);
        }
      }

      await dbContext.Payment.MapNote(paymentID, noteID);
    }

    async Task SendNotification(DbContext dbContext, long paymentID)
    {
      _eventBus.Publish(new SendNotificationWithPermissionsEvent
      {
        Title = await dbContext.Translation.GetTranslation(
          Locale.EN,
          "PaymentForReviewTitle"),
        Body = await dbContext.Translation.GetTranslation(
          Locale.EN,
          "PaymentForReview"),
        Module = "Payment",
        RefID = $"{paymentID}",
        Excludes = new string[] { _param.Officer.ID.ToString() },
        Permissions = new List<Permission> { Permission.PaymentReadWrite }
      });
    }
  }
}

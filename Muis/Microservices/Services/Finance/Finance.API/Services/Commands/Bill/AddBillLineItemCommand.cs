using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Finance.API.Extensions;
using Finance.API.Repository;
using Finance.Model;

namespace Finance.API.Services.Commands.Bill
{
  public class AddBillLineItemCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly IList<BillLineItem> _lineItems;

    public AddBillLineItemCommand(long id, IList<BillLineItem> lineItems)
    {
      _id = id;
      _lineItems = lineItems;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      if (_lineItems?.Any() ?? false)
      {
        var dbContext = DbContext.From(uow);
        uow.BeginTransaction();

        var bill = await dbContext.Bill.GetBillByID(_id);
        if (bill == null)
        {
          throw new NotFoundException();
        }

        var deductions = bill.LineItems.Where(e => e.SectionIndex >= 4)
          .OrderBy(e => e.Index)
          .ToList();

        bill.IssuedOn = DateTimeOffset.UtcNow;
        bill.LineItems = bill.LineItems.Where(e => e.SectionIndex <= 2).ToList();

        var index = bill.LineItems.Count() + 1;

        foreach (var lineItem in _lineItems)
        {
          lineItem.Index = index;
          lineItem.Section = "Others";
          lineItem.SectionIndex = 3;

          var transactionCode = await dbContext.Transactioncode.GetTransactionCodeByID(
            lineItem.CodeID);

          var price = transactionCode.GetLatestPriceAmount();

          var reverse = lineItem.Amount < 0 ? -1 : 1;

          var amount = price * lineItem.Qty.Value * reverse;
          var gstAmount = amount * bill.GST;

          lineItem.Amount = Math.Round(amount, 2);
          lineItem.GSTAmount = Math.Round(gstAmount, 2);
          lineItem.GST = bill.GST;

          bill.Amount += lineItem.Amount;
          bill.GSTAmount += lineItem.GSTAmount;

          bill.LineItems.Add(lineItem);

          index++;
        }

        // Reinsert deductions
        foreach (var lineItem in deductions)
        {
          lineItem.SectionIndex = 4;
          lineItem.Section = "Deductions";

          lineItem.Index = index;
          bill.LineItems.Add(lineItem);

          index++;
        }

        await dbContext.Bill.UpdateBill(bill);

        uow.Commit();
      }

      return Unit.Default;
    }
  }
}

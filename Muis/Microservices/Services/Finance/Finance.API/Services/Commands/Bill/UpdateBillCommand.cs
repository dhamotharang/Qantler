using System;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Bill
{
  public class UpdateBillCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Model.Bill _bill;

    public UpdateBillCommand(Model.Bill bill)
    {
      _bill = bill;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var existing = await dbContext.Bill.GetBillByID(_bill.ID);
      if (existing == null)
      {
        throw new NotFoundException();
      }

      var amount = 0M;
      var gstAmount = 0M;

      if (_bill.LineItems?.Any() ?? false)
      {
        foreach (var lineItem in _bill.LineItems)
        {
          amount += lineItem.Amount;
          gstAmount += lineItem.GSTAmount;
        }
      }

      _bill.Amount = Math.Round(amount, 2);
      _bill.GSTAmount = Math.Round(gstAmount, 2);

      await dbContext.Bill.UpdateBill(_bill);

      uow.Commit();

      return Unit.Default;
    }
  }
}

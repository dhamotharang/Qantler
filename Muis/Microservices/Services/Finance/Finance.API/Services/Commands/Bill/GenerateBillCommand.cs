using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Finance.API.Repository;
using Finance.API.Strategies.Billing;
using Finance.Events;

namespace Finance.API.Services.Commands.Bill
{
  public class GenerateBillCommand : IUnitOfWorkCommand<Model.Bill>
  {
    readonly BillRequest _request;

    readonly IEventBus _eventBus;

    public GenerateBillCommand(BillRequest request, IEventBus eventBus)
    {
      _request = request;
      _eventBus = eventBus;
    }

    public async Task<Model.Bill> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      uow.BeginTransaction();

      var strategy = new CertificateStrategy(dbContext, _request);

      var bill = await strategy.Generate();

      var billID = await dbContext.Bill.InsertBill(bill);

      bill = await dbContext.Bill.GetBillByID(billID);

      _eventBus.Publish(new OnBillGeneratedEvent
      {
        ID = bill.ID,
        RequestID = bill.RequestID ?? 0L,
        RefID = bill.RefID,
        RefNo = bill.RefNo,
        Type = bill.Type
      });

      uow.Commit();

      return bill;
    }
  }
}

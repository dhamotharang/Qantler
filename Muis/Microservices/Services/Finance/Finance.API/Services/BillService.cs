using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.EventBus;
using Finance.API.Services.Commands.Bill;
using Finance.API.Strategies.Billing;
using Finance.Model;

namespace Finance.API.Services
{
  public class BillService : TransactionalService,
                             IBillService
  {
    readonly IEventBus _eventBus;

    public BillService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _eventBus = eventBus;
    }

    public async Task<IList<Bill>> Filter(BillFilter filter)
    {
      return await Execute(new FilterBillCommand(filter));
    }

    public async Task<Bill> GenerateBill(BillRequest request)
    {
      return await Execute(new GenerateBillCommand(request, _eventBus));
    }

    public async Task UpdateBill(Bill bill)
    {
      await Execute(new UpdateBillCommand(bill));
    }

    public async Task AddLineItem(long id, IList<BillLineItem> lineItems)
    {
      await Execute(new AddBillLineItemCommand(id, lineItems));
    }

    public async Task<Bill> GetBillByID(long id)
    {
      return await Execute(new GetBillByIDCommand(id));
    }
  }
}

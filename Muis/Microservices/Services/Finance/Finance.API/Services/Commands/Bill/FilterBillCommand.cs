using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Bill
{
  public class FilterBillCommand : IUnitOfWorkCommand<IList<Model.Bill>>
  {
    readonly BillFilter _filter;

    public FilterBillCommand(BillFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Bill>> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);

      return await dbContext.Bill.Query(_filter);
    }
  }
}

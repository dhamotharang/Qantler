using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Customer
{
  public class FilterCustomerCommand : IUnitOfWorkCommand<IList<Model.Customer>>
  {
    readonly CustomerFilter _filter;

    public FilterCustomerCommand(CustomerFilter filter)
    {
      _filter = filter;
    }

    public Task<IList<Model.Customer>> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).Customer.Select(_filter);
    }
  }
}

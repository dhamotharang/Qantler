using Core.API;
using Core.API.Repository;
using Request.API.Models;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Customer
{
  public class SelectCustomerCommand : IUnitOfWorkCommand<IList<Model.Certificate360>>
  {
    readonly CustomerOptions _filter;

    public SelectCustomerCommand(CustomerOptions filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Certificate360>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Customer.QueryCustomer(_filter);
    }
  }
}
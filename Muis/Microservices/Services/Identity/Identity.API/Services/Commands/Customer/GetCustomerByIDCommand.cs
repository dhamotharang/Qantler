using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Customer
{
  public class GetCustomerByIDCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _id;

    public GetCustomerByIDCommand(Guid id)
    {
      _id = id;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Customer.GetCustomerByID(_id);
    }
  }
}

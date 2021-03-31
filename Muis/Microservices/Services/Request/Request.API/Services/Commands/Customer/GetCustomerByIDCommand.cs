using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Customer
{
  public class GetCustomerByIDCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _customerID;

    public GetCustomerByIDCommand(Guid customerID)
    {
      _customerID = customerID;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Customer.GetByID(_customerID);

      return result;
    }
  }
}
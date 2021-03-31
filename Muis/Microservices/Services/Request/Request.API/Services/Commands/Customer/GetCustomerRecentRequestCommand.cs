using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Customer
{
  public class GetCustomerRecentRequestCommand : IUnitOfWorkCommand<IEnumerable<Model.Request>>
  {
    readonly Guid _customerID;

    public GetCustomerRecentRequestCommand(Guid customerID)
    {
      _customerID = customerID;
    }

    public async Task<IEnumerable<Model.Request>> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Customer.GetRecentRequest(_customerID);

      return result;
    }
  }
}
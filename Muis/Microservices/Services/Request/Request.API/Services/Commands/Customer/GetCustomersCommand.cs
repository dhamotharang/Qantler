using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Customer
{
  public class GetCustomersCommand : IUnitOfWorkCommand<IList<Model.Customer>>
  {

    public GetCustomersCommand()
    {
    }

    public async Task<IList<Model.Customer>> Invoke(IUnitOfWork unitOfWork)
    {
      var result = await DbContext.From(unitOfWork).Customer.GetCustomers();

      return result;
    }
  }
}
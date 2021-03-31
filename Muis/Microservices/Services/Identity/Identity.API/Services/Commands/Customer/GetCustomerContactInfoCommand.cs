using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Customer
{
  public class GetCustomerContactInfoCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly string _name;
    readonly string _altID;

    public GetCustomerContactInfoCommand(string name, string altID)
    {
      _name = name;
      _altID = altID;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Customer.GetCustomerContactInfo(_name, _altID);
    }
  }
}

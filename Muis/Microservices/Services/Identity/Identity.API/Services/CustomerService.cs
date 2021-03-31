using System;
using System.Threading.Tasks;
using Identity.API.Services.Commands.Customer;
using Identity.Model;
using Core.API;
using Core.API.Provider;
using System.Collections.Generic;

namespace Identity.API.Services
{
  public class CustomerService : TransactionalService,
                                 ICustomerService
  {
    public CustomerService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<Customer> GetCustomerContactInfo(
      string name,
      string altID)
    {
      return await Execute(new GetCustomerContactInfoCommand(name, altID));
    }

    public async Task<Customer> CreateCustomer(Customer customer)
    {
      return await Execute(new CreateCustomer(customer));
    }

    public async Task<Customer> GetCustomerByID(Guid id)
    {
      return await Execute(new GetCustomerByIDCommand( id));
    }

    public Task<IList<Customer>> Filter(CustomerFilter filter)
    {
      return Execute(new FilterCustomerCommand(filter));
    }

    public Task<Customer> SetParent(Guid id, Guid parentID)
    {
      return Execute(new SetParentCommand(id, parentID));
    }
  }
}

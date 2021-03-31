using Core.API;
using Core.API.Provider;
using Request.API.Models;
using Request.API.Services.Commands.Customer;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public class CustomerService : TransactionalService,
                                 ICustomerService
  {
    public CustomerService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<Customer> SetCode(Guid customerID, long? codeID)
    {
      return await Execute(new SetCustomerCodeCommand(customerID, codeID));
    }

    public async Task<Customer> SetGroupCode(Guid customerID, long? groupCodeID)
    {
      return await Execute(new SetCustomerGroupCodeCommand(customerID, groupCodeID));
    }

    public async Task<Customer> SetOfficer(Guid customerID, Guid? officerInCharge)
    {
      return await Execute(new SetCustomerOfficerCommand(customerID, officerInCharge));
    }

    public async Task<Customer> GetByID(Guid customerID)
    {
      return await Execute(new GetCustomerByIDCommand(customerID));
    }

    public async Task<IList<Certificate360>> QueryCustomer(CustomerOptions filter)
    {
      return await Execute(new SelectCustomerCommand(filter));
    }

    public async Task<IEnumerable<Model.Request>> GetRecentRequest(Guid customerID)
    {
      return await Execute(new GetCustomerRecentRequestCommand(customerID));
    }

    public async Task<IList<Model.Customer>> GetCustomers()
    {
      return await Execute(new GetCustomersCommand());
    }

    public async Task<Model.Customer> CreateCustomer(Model.Customer data)
    {
      return await Execute(new CreateCustomerCommand(data));
    }
  }
}

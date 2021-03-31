using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Customer
{
  public class SetCustomerCodeCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _customerID;
    public long? _codeID;

    public SetCustomerCodeCommand(Guid customerID, long? codeID)
    {
      _customerID = customerID;
      _codeID = codeID;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var customer = await dbContext.Customer.GetByID(_customerID);
      if (customer == null)
      {
        throw new NotFoundException();
      }

      customer.CodeID = _codeID;

      await dbContext.Customer.UpdateCustomer(customer);

      await dbContext.Code.SyncCodeToRequest(customer.ID, _codeID, customer.GroupCodeID);

      uow.Commit();

      return customer;
    }
  }
}

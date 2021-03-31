using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Customer
{
  public class SetCustomerGroupCodeCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _customerID;
    public long? _groupCodeID;

    public SetCustomerGroupCodeCommand(Guid customerID, long? groupCodeID)
    {
      _customerID = customerID;
      _groupCodeID = groupCodeID;
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

      customer.GroupCodeID = _groupCodeID;

      await dbContext.Customer.UpdateCustomer(customer);

      await dbContext.Code.SyncCodeToRequest(customer.ID, customer.CodeID, _groupCodeID);

      uow.Commit();

      return customer;
    }
  }
}

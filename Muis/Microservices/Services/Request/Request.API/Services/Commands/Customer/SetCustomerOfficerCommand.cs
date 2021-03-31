using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Customer
{
  public class SetCustomerOfficerCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _customerID;
    public Guid? _officerInCharge;

    public SetCustomerOfficerCommand(Guid customerID, Guid? officerInCharge)
    {
      _customerID = customerID;
      _officerInCharge = officerInCharge;
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

      customer.OfficerInCharge = _officerInCharge;

      await dbContext.Customer.UpdateCustomer(customer);

      await dbContext.Code.SyncOfficerToRequest(customer.ID, customer.OfficerInCharge);

      uow.Commit();

      return customer;
    }
  }
}
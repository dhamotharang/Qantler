using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Customer
{
  public class SetParentCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Guid _id;
    readonly Guid _parentID;

    public SetParentCommand(Guid id, Guid parentID)
    {
      _id = id;
      _parentID = parentID;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var customer = await dbContext.Customer.GetCustomerByID(_id);
      if (customer == null)
      {
        throw new NotFoundException();
      }

      var parent = await dbContext.Customer.GetCustomerByID(_parentID);
      if (parent == null)
      {
        throw new NotFoundException();
      }

      customer.ParentID = parent.ID;
      customer.Parent = parent;

      await dbContext.Customer.UpdateCustomer(customer);

      uow.Commit();

      return customer;
    }
  }
}

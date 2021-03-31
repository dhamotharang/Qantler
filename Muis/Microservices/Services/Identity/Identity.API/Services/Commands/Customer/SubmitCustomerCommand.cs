using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Customer
{
  public class CreateCustomer : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Model.Customer _customer;

    public CreateCustomer(Model.Customer customer)
    {
      _customer = customer;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      CustomerValidator validators =
        new CustomerValidator(
          _customer.AltID,
          _customer.Name,
          dbContext);
      
      validators.Validate();

      _customer.ID = Guid.NewGuid();

      await dbContext.Customer.InsertCustomer(_customer);

      var result = await dbContext.Customer.GetCustomerByID(_customer.ID);

      unitOfWork.Commit();

      return result;
    }
  }
}

using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Customer
{
  public class CreateCustomerCommand : IUnitOfWorkCommand<Model.Customer>
  {
    readonly Model.Customer _customer;

    public CreateCustomerCommand(Model.Customer customer)
    {
      _customer = customer;
    }

    public async Task<Model.Customer> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var customers = await dbContext.Customer.GetCustomers();

      if (customers.Any(x => x.Name == _customer.Name
        && x.AltID == _customer.AltID))
      {
        var errorText = await dbContext.Transalation.GetTranslation(Locale.EN, "ValidateCustomer");

        throw new BadRequestException(errorText);
      }

      _customer.ID = Guid.NewGuid();

      await dbContext.Customer.InsertCustomer(_customer);

      var result = await dbContext.Customer.GetByID(_customer.ID);

      unitOfWork.Commit();

      return result;
    }
  }
}

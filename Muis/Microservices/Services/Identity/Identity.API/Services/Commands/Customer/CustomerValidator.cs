using Core.API;
using Core.API.Exceptions;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Customer
{
  public class CustomerValidator: Validator
  {
    readonly string _altID;
    readonly string _name;
    readonly DbContext _dbContext;

    public CustomerValidator(
      string AltID,
      string Name,
      DbContext dbContext)
    {
      _dbContext = dbContext;
      _altID = AltID;
      _name = Name;
    }

    protected override void DoValidate()
    {
      if (!_dbContext.Customer.ValidateCustomerStatus(_altID,_name))
      {
        throw new BadRequestException("Custormer already exists.");
      }
    }
  }
}

using Core.API;
using Core.API.Exceptions;
using Identity.API.Repository;

namespace Identity.API.Services.Commands.Person
{
  public class PersonValidator : Validator
  {
    readonly string _altID;
    readonly DbContext _dbContext;

    public PersonValidator(string altID, DbContext dbContext)
    {
      _dbContext = dbContext;
      _altID = altID;
    }

    protected override void DoValidate()
    {
      if (!_dbContext.Person.ValidatePersonStatus(_altID))
      {
        throw new BadRequestException("Person already exist.");
      }
    }
  }
}

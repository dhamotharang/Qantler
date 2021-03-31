using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Person
{
  public class InsertPersonCommand : IUnitOfWorkCommand<Model.Person>
  {
    readonly Model.Person _person;

    public InsertPersonCommand(Model.Person person)
    {
      _person = person;
    }

    public async Task<Model.Person> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      PersonValidator validators = new PersonValidator(
        _person.AltID, 
        dbContext);

      validators.Validate();

      _person.ID = Guid.NewGuid();

      await DbContext.From(unitOfWork).Person.InsertPerson(_person);

      var result = await DbContext.From(unitOfWork).Person.GetPersonByID(_person.ID);

      unitOfWork.Commit();

      return result;
    }
  }
}


 

 

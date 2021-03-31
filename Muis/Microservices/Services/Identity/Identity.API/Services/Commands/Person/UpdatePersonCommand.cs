using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Person
{
  public class UpdatePersonCommand : IUnitOfWorkCommand<Model.Person>
  {
    readonly Model.Person _person;

    public UpdatePersonCommand( Model.Person person)
    {
      _person = person;
    }

    public async Task<Model.Person> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Person.UpdatePerson(_person);
      var result = await dbContext.Person.GetPersonByID(_person.ID);

      unitOfWork.Commit();

      return result;
    }
  }
}

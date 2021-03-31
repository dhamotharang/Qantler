using Core.API;
using Core.API.Provider;
using Identity.API.Services.Commands;
using Identity.API.Services.Commands.Person;
using Identity.Model;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class PersonService : TransactionalService,
                               IPersonService
  {
    public PersonService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<Person> GetPersonByAltID(string altID)
    {
      return await Execute(new GetPersonByAltIDCommand(altID));
    }

    public async Task<Person> InsertPerson(Person person)
    {
      return await Execute(new InsertPersonCommand(person));
    }

    public async Task<Person> UpdatePerson(Person person)
    {
      return await Execute(new UpdatePersonCommand(person));
    }

    public async Task<Model.Person> GetPersonByID(Guid id)
    {
      return await Execute(new GetPersonByIDCommand(id));
    }
  }
}

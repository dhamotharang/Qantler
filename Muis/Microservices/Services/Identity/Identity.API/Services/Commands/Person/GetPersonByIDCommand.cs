using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Person
{
  public class GetPersonByIDCommand : IUnitOfWorkCommand<Model.Person>
  {
    readonly Guid _id;

    public GetPersonByIDCommand(Guid id)
    {
      _id = id;
    }

    public async Task<Model.Person> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Person.GetPersonByID(_id);
    }
  }
}

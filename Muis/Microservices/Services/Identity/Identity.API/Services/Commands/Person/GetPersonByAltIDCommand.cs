using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands
{
  public class GetPersonByAltIDCommand : IUnitOfWorkCommand<Model.Person>
  {
    readonly string _altID;

    public GetPersonByAltIDCommand(string altID)
    {
      _altID = altID;
    }

    public async Task<Model.Person> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Person.GetPersonByAltID(_altID);
    }
  }
}

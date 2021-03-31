using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using System.Threading.Tasks;

namespace Case.API.Services.Commands
{
  public class GetLeterByIDCommand : IUnitOfWorkCommand<Letter>
  {
    readonly long _letterID;

    public GetLeterByIDCommand(long letterID)
    {
      _letterID = letterID;
    }

    public async Task<Letter> Invoke(IUnitOfWork unitOfWork)
    {
      return (await DbContext.From(unitOfWork).Letter.GetLetterByID(_letterID));
    }
  }
}

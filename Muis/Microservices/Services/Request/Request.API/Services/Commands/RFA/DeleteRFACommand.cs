using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.RFA
{
  public class DeleteRFACommand : IUnitOfWorkCommand<bool>
  {
    readonly long _id;

    public DeleteRFACommand(long id)
    {
      _id = id;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.RFA.Delete(_id);

      unitOfWork.Commit();

      return true;
    }
  }
}

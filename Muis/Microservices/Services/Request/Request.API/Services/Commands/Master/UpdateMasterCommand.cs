using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Masterlist
{
  public class UpdateMasterCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Master _masterData;

    public UpdateMasterCommand(Master masterData)
    {
      _masterData = masterData;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();
      await dbContext.Master.UpdateMaster(_masterData);
      uow.Commit();
      return Unit.Default;
    }
  }
}

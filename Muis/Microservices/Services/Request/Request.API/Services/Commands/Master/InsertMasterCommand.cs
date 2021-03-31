using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Masterlist
{
  public class InsertMasterCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Master _masterData;

    public InsertMasterCommand(Master masterData)
    {
      _masterData = masterData;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();
      await dbContext.Master.InsertMaster(_masterData);
      uow.Commit();
      return Unit.Default;
    }
  }
}

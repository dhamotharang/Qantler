using Core.API;
using Core.API.Repository;
using Core.Model;
using JobOrder.API.Repository;
using System;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.Masterlist
{
  public class DeleteMasterCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Guid _id;

    public DeleteMasterCommand(Guid id)
    {
      _id = id;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();
      await dbContext.Master.DeleteMaster(_id);
      uow.Commit();
      return Unit.Default;
    }
  }
}

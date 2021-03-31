using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Code
{
  public class UpdateCodeCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Model.Code _entity;

    public UpdateCodeCommand(Model.Code entity)
    {
      _entity = entity;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.Code.Update(_entity);

      uow.Commit();

      return Unit.Default;
    }
  }
}

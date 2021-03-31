using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;

namespace Request.API.Services.Commands.Code
{
  public class CreateCodeCommand : IUnitOfWorkCommand<Model.Code>
  {
    readonly Model.Code _entity;

    public CreateCodeCommand(Model.Code entity)
    {
      _entity = entity;
    }

    public async Task<Model.Code> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var id = await dbContext.Code.Insert(_entity);
      _entity.ID = id;

      uow.Commit();

      return _entity;
    }
  }
}

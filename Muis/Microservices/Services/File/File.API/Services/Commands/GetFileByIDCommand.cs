using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using File.API.Repository;

namespace File.API.Services.Commands
{
  public class GetFileByIDCommand : IUnitOfWorkCommand<Model.File>
  {
    readonly Guid _id;

    public GetFileByIDCommand(Guid id)
    {
      _id = id;
    }

    public async Task<Model.File> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      return await dbContext.File.GetFileByID(_id);
    }
  }
}

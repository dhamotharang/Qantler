using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using File.API.Handlers;
using File.API.Repository;
using System;
using System.Threading.Tasks;

namespace File.API.Services.Commands
{
  public class DeleteCommand : IUnitOfWorkCommand<Unit>
  {
    readonly Guid _guid;
    readonly IFileHandler _fileHandler;

    public DeleteCommand(Guid guid, IFileHandler fileHandler)
    {
      _guid = guid;
      _fileHandler = fileHandler;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);

      var file = await dbContext.File.GetFileByID(_guid);
      if (file == null)
      {
        throw new NotFoundException();
      }

      await _fileHandler.Delete(file.Directory, file.ID.ToString());
      await dbContext.File.DeleteFile(_guid);

      return Unit.Default;
    }
  }
}

using System;
using System.Threading.Tasks;
using Core.API;
using File.API.Handlers;
using System.IO;
using Core.API.Exceptions;
using File.API.Repository;
using Core.API.Repository;

namespace File.API.Commands
{
  public class DownloadCommand : IUnitOfWorkCommand<Stream>
  {
    readonly Guid _guid;
    readonly IFileHandler _fileHandler;

    public DownloadCommand(Guid guid, IFileHandler fileHandler)
    {
      _guid = guid;
      _fileHandler = fileHandler;
    }
 
    public async Task<Stream> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);

      var file = await dbContext.File.GetFileByID(_guid);
      if (file == null)
      {
        throw new NotFoundException("");
      }
      
      return await _fileHandler.OpenRead(file.Directory, file.ID.ToString());
    }
  }
}

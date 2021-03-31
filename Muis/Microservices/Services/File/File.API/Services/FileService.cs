using System;
using File.API.Services;
using File.API.Commands;
using System.Threading.Tasks;
using System.IO;
using File.API.Config;
using File.API.Handlers;
using Microsoft.Extensions.Options;
using File.API.Services.Commands;
using Core.API;
using Core.API.Provider;

namespace File.Service
{
  public class FileService : TransactionalService,
                             IFileService
  {
    readonly FileConfig _fileConfig;
    readonly IFileHandler _fileHandler;

    public FileService(IDbConnectionProvider connectionProvider, IOptions<FileConfig> fileConfig)
         : base(connectionProvider)
    {
      _fileConfig = fileConfig.Value;

      _fileHandler = new LocalFileHandler(_fileConfig);

      if (_fileHandler == null)
      {
        throw new NotImplementedException($"{_fileConfig.Strategy} file strategy not supported.");
      }
    }

    public async Task<Model.File> Get(Guid guid)
    {
      return await Execute(new GetFileByIDCommand(guid));
    }

    public async Task<Model.File> Upload(
      string directory,
      string path,
      string extension,
      long size,
      string fileName = null)
    {
      return await Execute(new UploadCommand(_fileHandler, directory, path, extension, size,
        fileName));
    }

    public async Task<Stream> Download(Guid guid)
    {
      return await Execute(new DownloadCommand(guid, _fileHandler));
    }

    public async Task Delete(Guid guid)
    {
      await Execute(new DeleteCommand(guid, _fileHandler));
    }
  }
}

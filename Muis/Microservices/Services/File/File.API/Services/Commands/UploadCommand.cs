using System;
using System.IO;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using File.API.Handlers;
using File.API.Repository;

namespace File.API.Commands
{
  public class UploadCommand : IUnitOfWorkCommand<Model.File>
  {
    readonly IFileHandler _fileHandler;
    readonly string _directory;
    readonly string _path;
    readonly string _filename;
    readonly string _extension;
    readonly long _size;

    public UploadCommand(IFileHandler filehandler,
      string directory,
      string path,
      string extension,
      long size,
      string filename)
    {
      _fileHandler = filehandler;
      _directory = directory;
      _path = path;
      _filename = filename;
      _size = size;
      _extension = extension;
    }

    public async Task<Model.File> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);

      var id = Guid.NewGuid();

      await _fileHandler.Write(_path, _directory, id.ToString());

      var name = string.IsNullOrEmpty(_filename)
               ? Path.GetFileName(_path)
               : _filename;

      await dbContext.File.InsertFile(new File.Model.File
      {
        ID = id,
        FileName = name,
        Extension = _extension,
        Size = _size,
        Directory = _directory
      });

      return await dbContext.File.GetFileByID(id);
    }
  }
}

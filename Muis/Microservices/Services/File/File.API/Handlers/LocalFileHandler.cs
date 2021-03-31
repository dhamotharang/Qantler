using System.IO;
using System.Threading.Tasks;
using File.API.Config;


namespace File.API.Handlers
{
  public class LocalFileHandler : IFileHandler
  {
    readonly FileConfig _fileConfig;

    public LocalFileHandler(FileConfig config)
    {
      _fileConfig = config;
    }

    public Task<Stream> OpenRead(string directory, string fileName)
    {
      var path = string.IsNullOrEmpty(directory)
               ? $"{_fileConfig.Root}/{fileName}"
               : $"{_fileConfig.Root}/{directory}/{fileName}";

      return Task.Run(() => System.IO.File.OpenRead(path) as Stream);
    }

    public Task Write(string source, string directory, string fileName)
    {
      var dir = $"{_fileConfig.Root}/{directory}";
      Directory.CreateDirectory(dir);

      var destination = $"{dir}/{fileName}";
      System.IO.File.Copy(source, destination, true);

      return Task.CompletedTask;
    }

    public Task Delete(string directory, string fileName)
    {
      var path = string.IsNullOrEmpty(directory)
               ? $"{_fileConfig.Root}/{fileName}"
               : $"{_fileConfig.Root}/{directory}/{fileName}";
      
      System.IO.File.Delete(path);

      return Task.CompletedTask;
    }
  }
}

using System.IO;
using System.Threading.Tasks;

namespace File.API.Handlers
{
  public interface IFileHandler
  {
    Task<Stream> OpenRead(string directory, string fileName);

    Task Write(string source, string directory, string fileName);

    Task Delete(string directory, string fileName);
  }
}

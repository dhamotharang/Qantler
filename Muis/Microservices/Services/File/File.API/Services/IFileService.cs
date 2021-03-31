using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace File.API.Services
{
  public interface IFileService
  {
    /// <summary>
    /// Upload File.
    /// </summary>
    Task<Model.File> Upload(
      string directory,
      string path,
      string extension,
      long size,
      string fileName = null);

    /// <summary>
    /// Retrieve file base on Guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>The file info</returns>
    Task<Model.File> Get(Guid guid);

    /// <summary>
    /// Download file with specified id.
    /// </summary>
    Task<Stream> Download(Guid guid);

    /// <summary>
    /// Delete file with specified id.
    /// </summary>
    Task Delete(Guid guid);
  }
}

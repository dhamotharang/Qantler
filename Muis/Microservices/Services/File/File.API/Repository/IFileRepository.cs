using System;
using System.Threading.Tasks;


namespace File.API.Repository
{
  public interface IFileRepository
  {
    /// <summary>
    /// Insert file instance
    /// </summary>
    /// <param name="file">the file to insert</param>
    Task InsertFile(Model.File file);

    /// <summary>
    /// Retrieve file instance based on specified ID.
    /// </summary>
    /// <param name="guid">the file ID</param>
    /// <returns>the file instance, null if does not exist</returns>
    Task<Model.File> GetFileByID(Guid id);

    /// <summary>
    /// Delete file instance
    /// </summary>
    /// <param name="id">the file ID</param>
    Task DeleteFile(Guid id);
  }
}


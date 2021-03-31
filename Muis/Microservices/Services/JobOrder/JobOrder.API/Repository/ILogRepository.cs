using Core.Model;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface ILogRepository
  {
    /// <summary>
    /// Insert log entity.
    /// </summary>
    /// <param name="log">the log entity to insert</param>
    /// <returns>the ID of the inserted log entity</returns>
    Task<long> InsertLog(Log log);
  }
}

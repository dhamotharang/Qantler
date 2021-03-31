using System;
using System.Threading.Tasks;
using Core.Model;

namespace Finance.API.Repository
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

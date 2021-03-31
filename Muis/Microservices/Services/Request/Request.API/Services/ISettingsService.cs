using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface ISettingsService
  {
    /// <summary>
    /// Get settings
    /// </summary>
    /// <returns></returns>
    Task<List<Settings>> GetRequestSettings();

    /// <summary>
    /// update setting
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userID"></param>
    /// <param name="userName"></param>
    Task UpdateSettings(IList<Settings> settings, Guid userID, string userName);

  }
}

using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public interface ISettingsRepository
  {
    /// <summary>
    /// get settings
    /// </summary>
    /// <returns></returns>
    Task<List<Settings>> GetSettings();

    /// <summary>
    /// update settings 
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userID"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task UpdateSettings(IList<Settings> settings, Guid userID, string userName);

    /// <summary>
    /// Retrieve settings by type.
    /// </summary>
    Task<Settings> GetSettingsByType(SettingsType type);
  }
}

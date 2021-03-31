using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface ISettingService
  {
    /// <summary>
    /// Get settings
    /// </summary>
    /// <returns></returns>
    Task<List<Settings>> GetIdentitySettings();

    /// <summary>
    /// update settings
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userID"></param>
    /// <returns></returns>
    Task<bool> UpdateSettings(IList<Settings> settings, Guid userID, string userName);
  }
}

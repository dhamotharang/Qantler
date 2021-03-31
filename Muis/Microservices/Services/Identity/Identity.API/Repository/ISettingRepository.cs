using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public interface ISettingRepository
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
    Task<bool> UpdateSettings(IList<Settings> settings, Guid userID, string userName);
  }
}

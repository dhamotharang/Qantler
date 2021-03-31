using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface ISettingsRepository
  {
    /// <summary>
    /// get settings by id
    /// </summary>
    /// <returns></returns>
    Task<List<Settings>> GetSettings();

    /// <summary>
    /// update settings
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="userID"></param>
    /// <param name="userName"></param>
    Task UpdateSettings(IList<Settings> settings, Guid userID, string userName);
  }
}
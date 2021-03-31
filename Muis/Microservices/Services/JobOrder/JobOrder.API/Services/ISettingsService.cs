using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public interface ISettingsService
  {
    /// <summary>
    /// Get settings by id
    /// </summary>
    /// <returns></returns>
    Task<List<Settings>> GetJobOrderSettings();

    /// <summary>
    /// Update setting
    /// </summary>
    Task UpdateSettings(IList<Settings> settings, Guid userID, string userName);

  }
}

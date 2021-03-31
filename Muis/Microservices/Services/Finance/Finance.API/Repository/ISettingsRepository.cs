using System;
using System.Threading.Tasks;
using Finance.Model;

namespace Finance.API.Repository
{
  public interface ISettingsRepository
  {
    /// <summary>
    /// Retrieve settings instance base on specified type.
    /// </summary>
    /// <param name="type">the settings type</param>
    Task<Settings> GetSettingsByType(SettingsType type);
  }
}

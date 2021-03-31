using eHS.Portal.DTO;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Settings
{
  public interface ISettingsService
  {
    Task<SettingsDTO> GetSystemSettings();

    Task<string> UpdateSettings(SettingsDTO settings);
  }
}
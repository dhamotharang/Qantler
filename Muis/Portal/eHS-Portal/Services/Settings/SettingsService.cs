using eHS.Portal.Client;
using eHS.Portal.DTO;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Settings
{
  public class SettingsService : ISettingsService
  {
    readonly ApiClient _apiClient;

    public SettingsService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<SettingsDTO> GetSystemSettings()
    {
      SettingsDTO settings = new SettingsDTO
      {
        RequestSettings = await _apiClient.RequestSettingsSdk.GetSettings(),
        JobOrderSettings = await _apiClient.JobOrderSettingsSdk.GetSettings()
      };

      return settings;
    }

    public async Task<string> UpdateSettings(SettingsDTO settings)
    {
      var result = "";
      if (settings.RequestSettings.Count > 0)
      {
        result = await _apiClient.RequestSettingsSdk.UpdateSettings(settings.RequestSettings);
      }

      if (settings.JobOrderSettings.Count > 0)
      {
        result = await _apiClient.JobOrderSettingsSdk.UpdateSettings(settings.JobOrderSettings);
      }

      return result;
    }
  }
}
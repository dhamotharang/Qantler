using Core.API;
using Core.API.Provider;
using Identity.API.Services.Commands.Settings;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class SettingService : TransactionalService,
                                ISettingService
  {
    public SettingService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<List<Settings>> GetIdentitySettings()
    {
      return await Execute(new GetSettingsCommand());
    }

    public async Task<bool> UpdateSettings(
      IList<Settings> settings, Guid userID, string userName)
    {
      return await Execute(new UpdateSettingsCommand(settings, userID, userName));
    }
  }
}
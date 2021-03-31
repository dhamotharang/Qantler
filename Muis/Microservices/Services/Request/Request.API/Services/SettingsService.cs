using Core.API;
using Core.API.Provider;
using Request.API.Services.Commands.Settings;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public class SettingsService : TransactionalService,
                                 ISettingsService
  {
    public SettingsService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<List<Settings>> GetRequestSettings()
    {
      return await Execute(new GetSettingsCommand());
    }

    public async Task UpdateSettings(IList<Settings> settings, Guid userID, string userName)
    {
      await Execute(new UpdateSettingsCommand(settings, userID, userName));
    }
  }
}
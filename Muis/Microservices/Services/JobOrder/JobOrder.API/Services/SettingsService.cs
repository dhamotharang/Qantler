using Core.API;
using Core.API.Provider;
using JobOrder.API.Services.Commands.JobOrders.Settings;
using JobOrder.API.Services.Commands.Settings;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public class SettingsService : TransactionalService,
                                 ISettingsService
  {
    public SettingsService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<List<Settings>> GetJobOrderSettings()
    {
      return await Execute(new GetSettingsCommand());
    }

    public async Task UpdateSettings(IList<Settings> settings, Guid userID, string userName)
    {
      await Execute(new UpdateSettingsCommand(settings, userID, userName));
    }
  }
}

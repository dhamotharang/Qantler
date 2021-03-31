using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Request.API.Services.Commands.Statistics;
using Request.Model;

namespace Request.API.Services
{
  public class StatisticsService : TransactionalService,
                                   IStatisticsService
  {
    public StatisticsService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return Execute(new PerformanceCommand(keys, from, to));
    }

    public Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
     DateTimeOffset? to = null)
    {
      return Execute(new OverviewCommand(from, to));
    }

    public Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
     DateTimeOffset? to = null)
    {
      return Execute(new StatusCommand(from, to));
    }
  }
}

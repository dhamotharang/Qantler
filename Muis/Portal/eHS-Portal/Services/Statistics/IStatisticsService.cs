using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Statistics
{
  public interface IStatisticsService
  {
    Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null);

    Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null);
  }
}

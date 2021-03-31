using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Statistics
{
  public class StatisticsService : IStatisticsService
  {
    readonly ApiClient _apiClient;

    public StatisticsService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return _apiClient.RequestStatisticsSdk.Performance(keys, from, to);
    }

    public Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return _apiClient.RequestStatisticsSdk.Overview(from, to);
    }

    public Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return _apiClient.RequestStatisticsSdk.Status(from, to);
    }
  }
}

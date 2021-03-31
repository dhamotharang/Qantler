using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class RequestStatisticsSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public RequestStatisticsSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<StatisticsPerformance>> Performance(IList<string> keys,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/statistics/performance")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("from", from?.UtcDateTime.ToString())
        .AddParam("to", to?.UtcDateTime.ToString());

      if (keys?.Any() ?? false)
      {
        for (int i = 0; i < keys.Count; i++)
        {
          string key = keys[i];
          builder.AddParam($"keys[{i}]", key);
        }
      }

      return builder.Execute<IList<StatisticsPerformance>>();
    }

    public Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/statistics/overview")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("from", from?.UtcDateTime.ToString())
        .AddParam("to", to?.UtcDateTime.ToString())
        .Execute<IList<StatisticsOverview>>();
    }

    public Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/statistics/status")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("from", from?.UtcDateTime.ToString())
        .AddParam("to", to?.UtcDateTime.ToString())
        .Execute<IList<StatisticsStatus>>();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class NotificationSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public NotificationSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<IList<Notification>> List(DateTimeOffset? from)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/notification/list")
        .AddParam("from", from?.UtcDateTime.ToString())
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Notification>>();
    }

    public async Task<string> UpdateState(long id, State state)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/notification/{id}/state")
        .Method(HttpMethod.Put)
        .AddParam("state", $"{(int)state}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }
  }
}

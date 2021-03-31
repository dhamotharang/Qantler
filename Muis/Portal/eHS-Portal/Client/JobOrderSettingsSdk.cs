using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Http;
using System.Net.Http;
using eHS.Portal.Infrastructure.Providers;

namespace eHS.Portal.Client
{
  public class JobOrderSettingsSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public JobOrderSettingsSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<List<Settings>> GetSettings()
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/settings")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<List<Settings>>();
    }

    public Task<string> UpdateSettings(IList<Settings> settings)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/settings")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(settings))
        .Execute<string>();
    }
  }
}

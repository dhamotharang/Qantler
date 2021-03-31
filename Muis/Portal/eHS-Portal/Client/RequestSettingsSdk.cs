using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class RequestSettingsSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public RequestSettingsSdk(string url, IHttpRequestProvider requestProvider)
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

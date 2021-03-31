using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class MenuSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public MenuSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<string> UpdateAll(IList<Menu> menus)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/menu/list")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(menus))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public async Task<string> ReviewAll(IList<Menu> menus)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/menu/list/review")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(menus))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }
  }
}

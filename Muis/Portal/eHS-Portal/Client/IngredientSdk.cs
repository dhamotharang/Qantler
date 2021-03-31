using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class IngredientSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public IngredientSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<string> UpdateAll(IList<Ingredient> ingredients)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/ingredient/list")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(ingredients))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public async Task<string> ReviewAll(IList<Ingredient> ingredients)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/ingredient/list/review")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(ingredients))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }
  }
}

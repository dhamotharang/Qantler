using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class PremiseSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public PremiseSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<IList<Premise>> List(Guid? customerID)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/premise/query")
        .AddParam("customerID", $"{customerID.ToString()}")
        .AddInterceptor(new JsonDeserializerInterceptor());
      return await builder.Execute<IList<Premise>>();
    }

    public async Task<Premise> Create(Premise premise)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/premise")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(premise))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Premise>();
    }
  }
}

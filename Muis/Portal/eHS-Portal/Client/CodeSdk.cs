using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class CustomerCodeSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public CustomerCodeSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<string> Generate(CodeType type)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/code/generate")
        .AddParam("type", $"{(int)type}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<Code> Create(Code code)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/code")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(code))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Code>();
    }

    public Task<IList<Code>> List(CodeType type)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/code/list")
        .AddParam("type", $"{(int)type}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Code>>();
    }
  }
}

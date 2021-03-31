using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Providers;

namespace eHS.Portal.Client
{
  public class AuthSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public AuthSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<AuthResponse> Authenticate(AuthParam param)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri("/api/auth")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(param))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<AuthResponse>();
    }
  }
}

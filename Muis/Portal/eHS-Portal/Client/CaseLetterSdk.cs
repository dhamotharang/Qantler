using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class CaseLetterSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public CaseLetterSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<LetterTemplate> GetTemplate(LetterType type)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/letter/template/{(int)type}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<LetterTemplate>();
    }

    public Task<string> UpdateTemplate(LetterTemplate template)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/letter/template")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(template))
        .Execute<string>();
    }

    public async Task<Letter> GetLetterByID(long letterID)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/letter/{(int)letterID}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Letter>();
    }
  }
}

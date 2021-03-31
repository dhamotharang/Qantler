using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class CaseEmailSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public CaseEmailSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<EmailTemplate> GetTemplate(EmailTemplateType type)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/email/template/{(int)type}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<EmailTemplate>();
    }

    public async Task<Email> GetEmail(long id)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/email/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Email>();
    }

    public Task<string> UpdateTemplate(EmailTemplate emailTemplateDetails)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/email/template")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(emailTemplateDetails))
        .Execute<string>();
    }
  }
}

using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class ChecklistSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public ChecklistSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<ChecklistHistory> GetByID(long id)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/checklist/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<ChecklistHistory>();
    }

    public async Task<ChecklistHistory> GetLatest(Scheme scheme)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/checklist/latest")
        .AddParam("scheme", $"{(int)scheme}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<ChecklistHistory>();
    }

    public async Task<IList<ChecklistHistory>> GetByScheme(long id)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/checklist/scheme/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<ChecklistHistory>>();
    }

    public async Task<bool> InsertChecklist(ChecklistHistory data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/checklist")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(data))
        .Execute<bool>();
    }

    public async Task<bool> UpdateChecklist(ChecklistHistory data)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/checklist")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(data))
        .Execute<bool>();
    }
  }
}

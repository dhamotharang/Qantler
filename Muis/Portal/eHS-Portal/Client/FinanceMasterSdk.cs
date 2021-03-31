using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class FinanceMasterSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public FinanceMasterSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<IList<Model.Master>> GetMasterList(Model.MasterType type)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/Master")
       .AddParam("type", $"{(int)type}")
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<IList<Model.Master>>();
    }
  }
}

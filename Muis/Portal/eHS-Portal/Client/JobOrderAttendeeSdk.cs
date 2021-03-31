using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class JobOrderAttendeeSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public JobOrderAttendeeSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<Model.Attendee> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/attendee/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Model.Attendee>();
    }
  }
}

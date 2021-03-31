using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using eHS.Portal.Models.Cluster;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class ClusterSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public ClusterSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<Cluster>> Query()
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/Cluster/query")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Cluster>>();
    }

    public Task<Cluster> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/Cluster/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Cluster>();
    }

    public Task<long> AddCluster(Cluster cluster)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/Cluster")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(cluster))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<long>();
    }

    public Task<bool> UpdateCluster(Cluster cluster)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/Cluster")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(cluster))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<bool>();
    }

    public Task<bool> DeleteCluster(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/Cluster/{id}")
        .Method(HttpMethod.Delete)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<bool>();
    }
  }
}

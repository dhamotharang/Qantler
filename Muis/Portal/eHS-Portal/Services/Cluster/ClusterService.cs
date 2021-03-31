using Core.Http.Exceptions;
using eHS.Portal.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Cluster
{
  public class ClusterService : IClusterService
  {
    readonly ApiClient _apiClient;

    public ClusterService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<Model.Cluster>> Search()
    {
      return await _apiClient.ClusterSdk.Query();
    }

    public async Task<Model.Cluster> GetByID(long id)
    {
      var result = await _apiClient.ClusterSdk.GetByID(id);

      if (result == null)
      {
        throw new NotFoundException();
      }

      return result;
    }

    public async Task<long> AddCluster(Model.Cluster cluster)
    {
      return await _apiClient.ClusterSdk.AddCluster(cluster);
    }

    public async Task<bool> UpdateCluster(Model.Cluster cluster)
    {
      return await _apiClient.ClusterSdk.UpdateCluster(cluster);
    }

    public async Task<bool> DeleteCluster(long id)
    {
      return await _apiClient.ClusterSdk.DeleteCluster(id);
    }
  }
}
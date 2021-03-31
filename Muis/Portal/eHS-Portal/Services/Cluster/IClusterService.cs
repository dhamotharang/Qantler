using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Cluster
{
  public interface IClusterService
  {
    /// <summary>
    /// Get all cluster details
    /// </summary>
    /// <returns></returns>
    Task<IList<Model.Cluster>> Search();

    /// <summary>
    /// Get cluster by ClusterID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.Cluster> GetByID(long id);

    /// <summary>
    /// Add cluster by empty value
    /// </summary>
    /// <param name="cluster"></param>
    /// <returns></returns>
    Task<long> AddCluster(Model.Cluster cluster);

    /// <summary>
    /// update cluster for details page.
    /// </summary>
    /// <param name="cluster"></param>
    /// <returns></returns>
    Task<bool> UpdateCluster(Model.Cluster cluster);

    /// <summary>
    /// delete cluster by district id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteCluster(long i );
  }
}

using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public interface IClusterRepository
  {
    public Task<IEnumerable<Cluster>> Select();

    public Task<Cluster> GetClusterByID(long id);

    Task<long> AddCluster(Cluster cluster);

    Task<bool> UpdateCluster(Cluster cluster);

    Task<bool> DeleteCluster(long id);

    Task<IEnumerable<Cluster>> GetClusterByNode(string cNode);

    Task MapLog(long clusterID, long logID);
  }
}

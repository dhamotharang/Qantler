using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface IClusterService
  {
    Task<IEnumerable<Cluster>> QueryCluster();

    Task<Cluster> GetClusterByID(long id);

    Task<long> AddCluster(Cluster cluster, Guid userID, string userName);

    Task<bool> UpdateCluster(Cluster cluster, Guid userID, string userName);

    Task<bool> DeleteCluster(long id, Guid userID, string userName);
  }
}

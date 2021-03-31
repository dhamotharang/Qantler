using Core.API;
using Core.API.Provider;
using Identity.API.Services.Commands.Cluster;
using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class ClusterService : TransactionalService,
                                IClusterService
  {
    public ClusterService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<Cluster>> QueryCluster()
    {
      return await Execute(new QueryClusterCommand());
    }

    public async Task<Cluster> GetClusterByID(long id)
    {
      return await Execute(new GetClusterByIDCommand(id));
    }

    public async Task<long> AddCluster(Cluster cluster, Guid userID, string userName)
    {
      return await Execute(new AddClusterCommand(cluster, userID, userName));
    }

    public async Task<bool> UpdateCluster(Cluster cluster, Guid userID, string userName)
    {
      return await Execute(new UpdateClusterCommand(cluster, userID, userName));
    }

    public async Task<bool> DeleteCluster(long id, Guid userID, string userName)
    {
      return await Execute(new DeleteClusterCommand(id, userID, userName));
    }
  }
}

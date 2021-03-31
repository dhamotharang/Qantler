using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Core.Model;
using Identity.Model;
using System.Linq;

namespace Identity.API.Services.Commands.Identity
{
  public class GetCAToAssignAppCommand : IUnitOfWorkCommand<Model.Identity>
  {
    readonly IdentityFilter _filter;
    readonly string _clusterNode;

    public GetCAToAssignAppCommand(IdentityFilter filter, string ClusterNode)
    {
      _filter = filter;
      _clusterNode = ClusterNode;
    }

    public async Task<Model.Identity> Invoke(IUnitOfWork unitOfWork)
    {
      Model.Identity identityOfficer = null;
      Permission[] permissions = new Permission[] { Permission.RequestReview };
      _filter.Permissions = permissions;

      List<Model.Cluster> clusters = (List<Model.Cluster>)await DbContext.From(unitOfWork)
        .Cluster.GetClusterByNode(_clusterNode);
      if (clusters != null && clusters.Count > 0)
      {
        long[] clusts = new long[] { clusters[0].ID };
        _filter.Clusters = clusts;
        IList<Model.Identity> off = await DbContext.From(unitOfWork).Identity.Query(_filter);

        if (off != null && off.Count > 0)
        {
          List<Model.Identity> officers = new List<Model.Identity>(off);
          var rOfficers = officers.Where(o => o.Sequence == 0).ToList();
          if (rOfficers != null && rOfficers.Count > 0)
          {
            var assignedOfficer = rOfficers[0];
            assignedOfficer.Sequence = 1;
            await DbContext.From(unitOfWork).Identity.UpdateIdentitySequence(assignedOfficer);
            identityOfficer = assignedOfficer;
          }
          else
          {
            foreach (var item in officers)
            {
              item.Sequence = 0;
              await DbContext.From(unitOfWork).Identity.UpdateIdentitySequence(item);
            }
            var assignedOfficer = officers[0];
            assignedOfficer.Sequence = 1;
            await DbContext.From(unitOfWork).Identity.UpdateIdentitySequence(assignedOfficer);
            identityOfficer = assignedOfficer;
          }
        }
        else
        {
          // officer not available for the cluster - send push notification to admin       
        }
      }
      else
      {
        // cluster not available for the specific node - send push notification to admin
      }
      return identityOfficer;
    }
  }
}

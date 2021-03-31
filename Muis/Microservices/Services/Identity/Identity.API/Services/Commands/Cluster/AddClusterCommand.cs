using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Identity.API.Repository;
using Identity.API.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Cluster
{
  public class AddClusterCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.Cluster _cluster;
    readonly Guid _userID;
    readonly string _userName;

    public AddClusterCommand(Model.Cluster cluster, Guid userID, string userName)
    {
      _cluster = cluster;
      _userID = userID;
      _userName = userName;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var cluster = await dbContext.Cluster.Select();

      var duplicateCluster = cluster.Where(item => item.District == _cluster.District);

      var oldNodes = cluster.SelectMany(item => item.Nodes).Distinct().ToArray();

      var duplicateNodes = oldNodes.Intersect(_cluster.Nodes).ToList();

      var CurrentDuplicateNodes = _cluster.Nodes.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

      if (duplicateCluster.Count() != 0)
      {
        var errorText = await dbContext.Translation.GetTranslation(Locale.EN, "ValidateUniqueName");

        throw new BadRequestException(errorText);
      }
      else if (duplicateNodes.Count() != 0 || CurrentDuplicateNodes.Count() != 0)
      {
        var errorText = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "ValidateNode"),
          string.Join(",", duplicateNodes.Union(CurrentDuplicateNodes).ToArray()));

        throw new BadRequestException(errorText);
      }

      _cluster.Color = ClusterUtil.GetNextColor(cluster.Count());

      var id = await dbContext.Cluster.AddCluster(_cluster);

      var logText = await dbContext.Translation.GetTranslation(Locale.EN, "AddNewCluster");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.Default,
        Action = logText,
        UserID = _userID,
        UserName = _userName
      });

      await dbContext.Cluster.MapLog(id, logID);

      unitOfWork.Commit();

      return id;
    }
  }
}

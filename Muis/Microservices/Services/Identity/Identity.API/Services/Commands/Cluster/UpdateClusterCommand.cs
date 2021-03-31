using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Identity.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Cluster
{
  public class UpdateClusterCommand : IUnitOfWorkCommand<bool>
  {
    readonly Model.Cluster _cluster;
    readonly Guid _userID;
    readonly string _userName;

    public UpdateClusterCommand(Model.Cluster cluster, Guid userID, string userName)
    {
      _cluster = cluster;
      _userID = userID;
      _userName = userName;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var cluster = await dbContext.Cluster.Select();

      var duplicateCluster = cluster.Where(item => item.District == _cluster.District && item.ID != _cluster.ID);

      var oldNodes = cluster.Where(item => item.ID != _cluster.ID).SelectMany(item => item.Nodes).Distinct().ToArray();

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

      var addedNode = _cluster.Nodes.ToArray().Except(cluster.Where(item => item.ID == _cluster.ID).First().Nodes.ToArray());

      var deletedNode = cluster.Where(item => item.ID == _cluster.ID).First().Nodes.ToArray().Except(_cluster.Nodes.ToArray());

      var result = await dbContext.Cluster.UpdateCluster(_cluster);

      var logText = "";
      var logParam = new List<string>();

      if (addedNode.Count() != 0)
      {
        logText = await dbContext.Translation.GetTranslation(Locale.EN, "AddNewNode");
        logParam.Add(string.Join(",", addedNode));
      }

      if (deletedNode.Count() != 0)
      {
        var deleteLogText = await dbContext.Translation.GetTranslation(Locale.EN, "DeleteNode");
        if (!string.IsNullOrEmpty(logText))
        {
          logText += "<br>";
          deleteLogText = deleteLogText.Replace("{0}", "{1}");
        }

        logParam.Add(string.Join(",", deletedNode));
        logText += deleteLogText;
      }

      if (!string.IsNullOrEmpty(logText))
      {
        var logID = await dbContext.Log.InsertLog(new Log
        {
          Type = LogType.Default,
          Action = logText,
          UserID = _userID,
          UserName = _userName,
          Params = logParam.ToArray()
        });

        await dbContext.Cluster.MapLog(_cluster.ID, logID);
      }


      unitOfWork.Commit();

      return result;
    }
  }
}

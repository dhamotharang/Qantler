using Core.API;
using Core.API.Repository;
using Core.Model;
using Identity.API.Repository;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Cluster
{
  public class DeleteClusterCommand : IUnitOfWorkCommand<bool>
  {
    readonly long _id;

    readonly Guid _userID;
    readonly string _userName;

    public DeleteClusterCommand(long id, Guid userID, string userName)
    {
      _id = id;
      _userID = userID;
      _userName = userName;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var cluster = await dbContext.Cluster.GetClusterByID(_id);

      var result = await dbContext.Cluster.DeleteCluster(_id);

      var logText = await dbContext.Translation.GetTranslation(Locale.EN, "DeleteCluster");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.Default,
        Action = logText,
        UserID = _userID,
        UserName = _userName,
        Params = new string[]
        {
           cluster.District
        }
      });

      await dbContext.Cluster.MapLog(_id, logID);

      unitOfWork.Commit();

      return result;
    }
  }
}
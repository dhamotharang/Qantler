using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Notification.Model;

namespace Notification.API.Repository
{
  public class NotificationRepository : INotificationRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public NotificationRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Model.Notification> GetByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Model.Notification>(_unitOfWork.Connection,
        "GetNotificationByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction))?.FirstOrDefault();
    }

    public async Task<long> InsertNotification(Model.Notification entity, IList<Guid> toUserIDs)
    {
      var param = new DynamicParameters();
      param.Add("@Title", entity.Title);
      param.Add("@Preview", entity.Preview);
      param.Add("@Body", entity.Body);
      param.Add("@RefID", entity.RefID);
      param.Add("@Module", entity.Module);
      param.Add("@Category", entity.Category);
      param.Add("@Level", entity.Level);
      param.Add("@ContentType", entity.ContentType);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertNotification",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var result = param.Get<long>("@ID");

      if (toUserIDs?.Any() ?? false)
      {
        var userNotifParam = new DynamicParameters();
        userNotifParam.Add("@NotificationID", result);

        var userType = new DataTable();
        userType.Columns.Add("Val");

        foreach (var userID in toUserIDs)
        {
          userType.Rows.Add(userID);
        }

        userNotifParam.Add("@UserIDs", userType.AsTableValuedParameter("dbo.UniqueIdentifierType"));

        await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
          "MapUserToNofication",
          userNotifParam,
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction);
      }

      return result;
    }

    public async Task<IList<Model.Notification>> Query(NotificationFilter filter)
    {
      var param = new DynamicParameters();
      param.Add("@UserID", filter.UserID);
      param.Add("@From", filter.From);
      param.Add("@LastModified", filter.From);

      return (await SqlMapper.QueryAsync<Model.Notification>(_unitOfWork.Connection,
        "SelectNotification",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task UpdateState(long notificationID, Guid userID, State state)
    {
      var param = new DynamicParameters();
      param.Add("@NotificationID", notificationID);
      param.Add("@UserID", userID);
      param.Add("@State", state);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateNotificationState",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task Clear(Guid userID)
    {
      var param = new DynamicParameters();
      param.Add("@UserID", userID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteAllNotification",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}

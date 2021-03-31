using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class ReauditRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly string _notes;

    readonly Guid _userID;
    readonly string _userName;

    readonly IEventBus _eventBus;

    public ReauditRequestCommand(long id, string notes, Guid userID,
      string userName, IEventBus eventBus)
    {
      _id = id;
      _notes = notes;

      _userID = userID;
      _userName = userName;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var actionHistories = await dbContext.Request.GetRequestActionHistories(_id);
      var lastAuditAction = actionHistories.Where(e => e.Action == RequestActionType.Review)
        .OrderByDescending(e => e.CreatedOn)
        .First();

      await dbContext.Request.SetAssignedOfficer(_id,
        lastAuditAction.Officer.ID,
        lastAuditAction.Officer.Name);

      await dbContext.Request.InsertActionHistory(new RequestActionHistory
      {
        Action = RequestActionType.Reaudit,
        RequestID = _id,
        Officer = new Officer
        {
          ID = _userID,
          Name = _userName
        }
      });

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestReaudit"),
        Notes = _notes,
        UserID = _userID,
        UserName = _userName
      });

      await dbContext.Request.MapLog(_id, logID);

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestReauditNotifTitle"),
        Body = await dbContext.Transalation.GetTranslation(Locale.EN, "RequestReauditNotifMsg"),
        RefID = $"{_id}",
        Module = "Request",
        To = new List<string>() { lastAuditAction.Officer.ID.ToString() }
      });

      await dbContext.Request.UpdateStatus(_id, RequestStatus.Open, null);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

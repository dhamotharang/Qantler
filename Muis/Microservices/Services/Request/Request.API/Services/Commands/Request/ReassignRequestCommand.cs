using System;
using System.Collections.Generic;
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
  public class ReassignRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;
    readonly Officer _toOfficer;
    readonly Officer _user;
    readonly string _notes;

    readonly IEventBus _eventBus;

    public ReassignRequestCommand(long requestID, Officer toOfficer, string notes, Officer user,
      IEventBus eventBus)
    {
      _requestID = requestID;
      _toOfficer = toOfficer;
      _user = user;
      _notes = notes;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Request.SetAssignedOfficer(_requestID, _toOfficer.ID, _toOfficer.Name);

      var logText = string.Format(
        await dbContext.Transalation.GetTranslation(0, "RequestReassign"),
        _toOfficer.Name);

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = logText,
        Notes = _notes,
        UserID = _user.ID,
        UserName = _user.Name,
      });

      await dbContext.Request.MapLog(_requestID, logID);

      await dbContext.Request.InsertActionHistory(new RequestActionHistory
      {
        Action = RequestActionType.Reassign,
        RequestID = _requestID,
        Officer = _user
      });

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Transalation.GetTranslation(0, "RequestReassignNotifTitle"),
        Body = await dbContext.Transalation.GetTranslation(0, "RequestReassignNotifBody"),
        RefID = $"{_requestID}",
        Module = "Request",
        To = new List<string> { _toOfficer.ID.ToString() }
      });

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

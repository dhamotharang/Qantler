using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class ReinstateRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _requestID;
    readonly Officer _user;
    readonly string _notes;
    readonly Master _reason;

    readonly IEventBus _eventBus;

    public ReinstateRequestCommand(long requestID, string notes, Officer user,
      Master reason, IEventBus eventBus)
    {
      _requestID = requestID;
      _user = user;
      _notes = notes;
      _reason = reason;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var request = await dbContext.Request.GetRequestByID(_requestID);
      if (request == null)
      {
        throw new BadRequestException(
         await dbContext.Transalation.GetTranslation(Locale.EN, "RequestNotFound"));
      }

      await dbContext.Request.UpdateStatus(request.ID,
        request.OldStatus.Value,
         request.StatusMinor, null);

      var logText = string.Format(
        await dbContext.Transalation.GetTranslation(0, "ReinstateRequest"),
        _user.Name);

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = logText,
        Notes = string.Format("Reason: {0} Notes: {1}", _reason.Value, _notes),
        UserID = _user.ID,
        UserName = _user.Name,
      });

      await dbContext.Request.MapLog(_requestID, logID);

      await dbContext.Request.InsertActionHistory(new RequestActionHistory
      {
        Action = RequestActionType.Reinstate,
        RequestID = _requestID,
        Officer = _user,
        RefID = _reason.ID.ToString(),
        Remarks = _reason.Value
      });

      if (request.AssignedTo != _user.ID)
      {
        _eventBus.Publish(new SendNotificationEvent
        {
          Title = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationTitle"),
          Body = await dbContext.Transalation.GetTranslation(0, "NewRequestNotificationBody"),
          RefID = $"{_requestID}",
          Module = "Request",
          To = new List<string> { request.AssignedTo.ToString() }
        });
      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

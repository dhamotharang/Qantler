using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.RFA
{
  public class UpdateRFAStatusCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly Guid _userID;
    readonly string _userName;
    readonly RFAStatus _rfastatus;

    readonly IEventBus _eventBus;

    public UpdateRFAStatusCommand(long id, Guid userID, string username, RFAStatus rfastatus,
      IEventBus eventBus)
    {
      _id = id;
      _userID = userID;
      _userName = username;
      _rfastatus = rfastatus;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var rfa = await dbContext.RFA.GetRFAByID(_id);
      if (_rfastatus <= rfa.Status)
      {
        throw new BadRequestException(
          await dbContext.Transalation.GetTranslation(Locale.EN, "Outdated"));
      }

      await dbContext.RFA.UpdateRFAStatus(_id, _rfastatus, _userID, _userName);

      if (_rfastatus == RFAStatus.Closed
          && _userID != rfa.RaisedBy)
      {
        _eventBus.Publish(new SendNotificationEvent
        {
          Module = "Request",
          RefID = $"{rfa.RequestID}",
          Title = await dbContext.Transalation.GetTranslation(Locale.EN, "RFAProcessedByTitle"),
          Body = string.Format(
            await dbContext.Transalation.GetTranslation(Locale.EN, "RFAProcessedByBody"),
            _userName),
          To = new string[] { rfa.RaisedBy.ToString() }
        });
      }
      else if (_rfastatus == RFAStatus.Expired)
      {
        Model.Request request = dbContext.Request.GetRequestByID(rfa.RequestID).Result;
        if (request != null && request.ID > 0)
        {
          var logtext = await dbContext.Transalation.GetTranslation(Locale.EN,
            "ExpireRequestLapseRFABody");
          if (!string.IsNullOrEmpty(logtext))
          {
            var logid = await dbContext.Log.InsertLog(new Log
            {
              Action = logtext,
              UserID = (Guid)request.AssignedTo,
              UserName = request.AssignedToName
            });
            await dbContext.Request.MapLog(request.ID, logid);
          }

          _eventBus.Publish(new SendNotificationEvent
          {
            Title = await dbContext.Transalation.GetTranslation(0, "ExpireRequestTitle"),
            Body = await dbContext.Transalation.GetTranslation(0, "ExpireRequestLapseRFABody"),
            Module = "Request",
            To = new List<string> { request.AssignedTo.ToString() }
          });

          await dbContext.Request.UpdateStatus(request.ID, RequestStatus.Expired, null);

        }

        var logText = string.Format(
       await dbContext.Transalation.GetTranslation(0, "RFALapsedBody"));

        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = logText
        });
      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

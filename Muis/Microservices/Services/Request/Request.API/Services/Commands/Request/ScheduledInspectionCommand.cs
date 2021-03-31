using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class ScheduledInspectionCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly long _jobID;
    readonly DateTimeOffset _scheduleOn;
    readonly DateTimeOffset _scheduleOnTo;
    readonly string _notes;

    readonly Guid _userID;
    readonly string _userName;

    public ScheduledInspectionCommand(long id, long jobID, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes, Guid userID, string userName)
    {
      _id = id;
      _jobID = jobID;
      _scheduleOn = scheduledOn;
      _scheduleOnTo = scheduledOnTo;
      _notes = notes;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Request.MapJobOrderToRequest(_id, _jobID);

      await dbContext.Request.UpdateStatus(_id, RequestStatus.ForInspection, null);

      var logText = await dbContext.Transalation.GetTranslation(Locale.EN,
        "RequestScheduleInspection");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.JobOrder,
        RefID = $"{_jobID}",
        Notes = _notes,
        Action = logText,
        Params = new string[]
        {
          _scheduleOn.UtcDateTime.ToString(),_scheduleOnTo.UtcDateTime.ToString()
        },
        UserID = _userID,
        UserName = _userName
      });

      await dbContext.Request.MapLog(_id, logID);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

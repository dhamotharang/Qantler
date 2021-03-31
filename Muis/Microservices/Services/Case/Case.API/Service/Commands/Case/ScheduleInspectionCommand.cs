using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class ScheduleInspectionCommand : IUnitOfWorkCommand<long>
  {
    readonly Officer _user;
    readonly long _caseID;
    readonly CaseScheduleInspectionParam _data;

    public ScheduleInspectionCommand(long caseID, CaseScheduleInspectionParam data, Officer user)
    {
      _data = data;
      _user = user;
      _caseID = caseID;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      await dbContext.User.InsertOrReplace(_user);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "ScheduledInspectionNote");

      var activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = await dbContext.Translation.GetTranslation(Locale.EN, "ScheduledInspection"),
        RefID = _data.JobOrderID.ToString(),
        Type = ActivityType.JobOrder,
        User = _user,
        Notes = string.Format(action, _data.PremisesText, DateTimeToDate(_data.ScheduledOn.Value),
                        DateTimeToTime(_data.ScheduledOn.Value), DateTimeToTime(_data.ScheduledOnTo.Value),
                        _data.Notes),
      }, _caseID);

      if (_data.Type.Value == JobOrderType.Reinstate)
      {
        await dbContext.Case.UpdateStatus(_caseID, @case.Status, CaseMinorStatus.InspectionInProgress);
      }
      else if (_data.Type.Value == JobOrderType.Enforcement)
      {
        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingInspection);
      }

      unitOfWork.Commit();
      return activityID;
    }

    public static string DateTimeToTime(DateTimeOffset value)
    {
      return value.ToString("hh:mm tt");
    }

    public static string DateTimeToDate(DateTimeOffset value)
    {
      var date = new StringBuilder();
      date.Append(value.ToString("MMM dd"));
      date.Append(", ");
      date.Append(value.ToString("yyyy"));
      return date.ToString();
    }
  }
}
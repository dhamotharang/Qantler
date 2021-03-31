using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseFOCRevertCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly ReviewFOCParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseFOCRevertCommand(long caseID, ReviewFOCParam data, Officer user, IEventBus eventBus)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
      _eventBus = eventBus;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingFOC);

      var @case = await dbContext.Case.GetBasicByID(_caseID);
      @case.AssignedToID = null;

      await dbContext.Case.UpdateCaseInfo(@case);

      var lastActivity = (await dbContext.Case.GetActivityByCaseID(_caseID))?.
            FirstOrDefault(x => x.Type == ActivityType.FOCLetter);

      var draftAction = await dbContext.Translation.GetTranslation(Locale.EN, "FOCApproverDraft");

      if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
      {
        await dbContext.Activity.DeleteByID(lastActivity.ID);
      }

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCApproverRevert");

      var activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.FOCLetter,
        Action = action,
        User = _user,
        Notes = _data.Notes
      }, _caseID);

      if (_data.Letter != null && _data.Letter.ID != 0)
      {
        _data.Letter.Status = LetterStatus.Final;
        await dbContext.Letter.UpdateLetter(_data.Letter);
      }

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Translation.GetTranslation(Locale.EN, "InviteFOCRevertTitle"),
        Body = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "InviteFOCRevertBody"),
          $"{_data.Notes}"),
        Module = "Case",
        RefID = $"{_caseID}",
        To = new string[] { @case.ManagedByID.ToString() }
      });

      unitOfWork.Commit();

      return activityID;
    }
  }
}
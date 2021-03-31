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
  public class CaseFOCFinalLetterCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly CaseFOCParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseFOCFinalLetterCommand(long caseID, CaseFOCParam data, Officer user, IEventBus eventBus)
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

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      var action = "";
      var draftAction = "";

      _data.Letter.Type = LetterType.FOC;

      if (_data.Letter.ID == 0)
      {
        _data.Letter.ID = await dbContext.Letter.InsertLetter(_data.Letter);
      }
      else
      {
        await dbContext.Letter.UpdateLetter(_data.Letter);
      }

      var lastActivity = (await dbContext.Case.GetActivityByCaseID(_caseID))?.
            FirstOrDefault(x => x.Type == ActivityType.FOCLetter);

      if (@case.Sanction == Sanction.Reinstate)
      {
        draftAction = await dbContext.Translation.GetTranslation(Locale.EN, "FOCCaseFileStatusDraft");
      }
      else
      {
        draftAction = await dbContext.Translation.GetTranslation(Locale.EN, "FOCStatusDraft");
      }

      if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
      {
        await dbContext.Activity.DeleteByID(lastActivity.ID);
      }

      if (@case.Sanction == Sanction.Reinstate)
      {
        action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCCaseFileStatusFinal");
      }
      else
      {
        action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCStatusFinal");
      }

      var activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.FOCLetter,
        Action = action,
        User = _user
      }, _caseID);

      await dbContext.Activity.MapActivityLetter(activityID, _data.Letter.ID);

      @case.AssignedToID = _user.ID;
      @case.Status = CaseStatus.FOCForApproval;
      @case.MinorStatus = null;

      await dbContext.Case.UpdateCaseInfo(@case);

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Translation.GetTranslation(Locale.EN, "InviteFOCFinalTitle"),
        Body = await dbContext.Translation.GetTranslation(Locale.EN, "InviteFOCFinalBody"),
        Module = "Case",
        RefID = $"{_caseID}",
        To = _data.Reviewer.Select(x => x.ID.ToString()).ToList()
      });

      unitOfWork.Commit();

      return _data.Letter.ID;
    }
  }
}
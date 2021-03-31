using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.Base;
using Core.Model;
using Core.EventBus;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class SanctionCompositionSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly IEventBus _eventBus;
    readonly Officer _user;
    readonly long _letterID;

    public SanctionCompositionSubCommand(DbContext dbContext, long caseID,
      Officer user, long letterID, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _eventBus = eventBus;
      _user = user;
      _letterID = letterID;
    }
    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);
      var offender = await _dbContext.Offender.GetOffenderByID(@case.OffenderID.Value);

      await _dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingPayment);

      var lastActivity = (await _dbContext.Case.GetActivityByCaseID(_caseID))?.
          FirstOrDefault(x => x.Type == ActivityType.SanctionLetter);

      var draftAction = await _dbContext.Translation.GetTranslation(Locale.EN, "CompoundDraftLetter");

      if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
      {
        await _dbContext.Activity.DeleteByID(lastActivity.ID);
      }

      var action = await _dbContext.Translation.GetTranslation(Locale.EN, "CompoundFinalLetter");

      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.SanctionLetter,
        Action = action,
        User = _user
      }, _caseID);

      await _dbContext.Activity.MapActivityLetter(activityID, _letterID);

      var sanctionInfo = await _dbContext.Sanction.GetSanctionInfo(_caseID);
      var amount = sanctionInfo.Where(x => x.Sanction == Sanction.Compound
        && x.Type == SanctionInfoType.Final)
        .OrderByDescending(x => x.ID)
        .FirstOrDefault().Value;

      _eventBus.Publish(new RequestCompositionBillEvent
      {
        AccountID = offender.ID,
        Name = offender.Name,
        Amount = decimal.Parse(amount),
        RefID = @case.ID
      });
    }
  }
}

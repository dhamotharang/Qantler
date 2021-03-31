using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.Base;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class ReinstateDecisionClosedSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly IEventBus _eventBus;
    readonly Officer _user;
    readonly ReinstateDecisionParam _data;

    public ReinstateDecisionClosedSubCommand(DbContext dbContext,
      long caseID, Officer user, ReinstateDecisionParam data, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _eventBus = eventBus;
      _data = data;
      _user = user;
    }
    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);

      await _dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed);

      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Action = await _dbContext.Translation.GetTranslation(Locale.EN, "ReinstateClosed"),
        User = _user
      }, _caseID);

      if (_data.Attachment?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachment)
        {
          attachment.ID = await _dbContext.Attachment.InsertAttachment(attachment);
        }

        await _dbContext.Activity.MapActivityAttachments(
          activityID,
          _data.Attachment.Select(e => e.ID).ToArray());
      }

      var premises = await _dbContext.Premise.GetPremises(_caseID);

      _eventBus.Publish(new OnCaseStatusChangedEvent
      {
        NewStatus = CaseStatus.Closed,
        OldStatus = @case.Status,
        PremisesID = premises.Select(x => x.ID).ToList()
      });
    }
  }
}

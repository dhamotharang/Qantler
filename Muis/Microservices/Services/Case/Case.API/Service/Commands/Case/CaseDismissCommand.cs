using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseDismissCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly CaseDismissParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseDismissCommand(long caseID, CaseDismissParam data, Officer user, IEventBus eventBus)
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

      await dbContext.Case.UpdateStatus(_caseID, CaseStatus.Dismissed);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "CaseDismissed");

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        User = _user,
        Notes = _data.Notes
      }, _caseID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          _activityID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      var premises = await dbContext.Premise.GetPremises(_caseID);

      _eventBus.Publish(new OnCaseStatusChangedEvent
      {
        NewStatus = CaseStatus.Closed,
        OldStatus = @case.Status,
        PremisesID = premises.Select(x => x.ID).ToList()
      });

      unitOfWork.Commit();

      return _activityID;
    }
  }
}
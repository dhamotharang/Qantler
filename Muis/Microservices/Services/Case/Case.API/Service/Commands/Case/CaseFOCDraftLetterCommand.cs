using Case.API.Params;
using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseFOCDraftLetterCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly CaseFOCParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseFOCDraftLetterCommand(long caseID, CaseFOCParam data, Officer user, IEventBus eventBus)
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

      _data.Letter.Type = LetterType.FOC;

      if (_data.Letter.Status == LetterStatus.Draft && _data.Letter.ID == 0)
      {
        _data.Letter.ID = await dbContext.Letter.InsertLetter(_data.Letter);

        var action = "";

        if (@case.Sanction == Sanction.Reinstate)
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCCaseFileStatusDraft");
        }
        else
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCStatusDraft");
        }

        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingFOC);

        var activityID = await dbContext.Activity.InsertActivity(new Activity
        {
          Type = ActivityType.FOCLetter,
          Action = action,
          User = _user
        }, _caseID);

        await dbContext.Activity.MapActivityLetter(activityID, _data.Letter.ID);
      }
      else
      {
        await dbContext.Letter.UpdateLetter(_data.Letter);
      }

      unitOfWork.Commit();

      return _data.Letter.ID;
    }
  }
}
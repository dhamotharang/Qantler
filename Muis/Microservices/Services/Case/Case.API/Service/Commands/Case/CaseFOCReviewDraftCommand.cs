using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseFOCReviewDraftCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly ReviewFOCParam _data;
    readonly Officer _user;

    public CaseFOCReviewDraftCommand(long caseID, ReviewFOCParam data, Officer user)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      _data.Letter.Type = LetterType.FOC;

      if (_data.Letter.ID != 0)
      {
        await dbContext.Letter.UpdateLetter(_data.Letter);
      }
      else
      {
        _data.Letter.ID = await dbContext.Letter.InsertLetter(_data.Letter);

        var action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCApproverDraft");

        var activityID = await dbContext.Activity.InsertActivity(new Activity
        {
          Type = ActivityType.FOCLetter,
          Action = action,
          User = _user,
        }, _caseID);

        await dbContext.Activity.MapActivityLetter(activityID, _data.Letter.ID);
      }

      unitOfWork.Commit();

      return _data.Letter.ID;
    }
  }
}
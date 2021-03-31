using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseSanctionDraftLetterCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly Letter _letter;
    readonly Officer _user;

    public CaseSanctionDraftLetterCommand(long caseID, Letter letter, Officer user)
    {
      _caseID = caseID;
      _letter = letter;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      if (_letter.Status == LetterStatus.Draft && _letter.ID == 0)
      {
        _letter.ID = await dbContext.Letter.InsertLetter(_letter);

        var action = "";

        if (_letter.Type == LetterType.Compound)
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "CompoundDraftLetter");
        }
        else if (_letter.Type == LetterType.Revocation)
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "RevocationDraftLetter");
        }
        else if (_letter.Type == LetterType.Suspension
          || _letter.Type == LetterType.ImmediateSuspension)
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "SuspensionDraftLetter");
        }
        else if (_letter.Type == LetterType.Warning)
        {
          action = await dbContext.Translation.GetTranslation(Locale.EN, "WarningDraftLetter");
        }

        var activityID = await dbContext.Activity.InsertActivity(new Activity
        {
          Type = ActivityType.SanctionLetter,
          Action = action,
          User = _user
        }, _caseID);

        await dbContext.Activity.MapActivityLetter(activityID, _letter.ID);
      }
      else
      {
        await dbContext.Letter.UpdateLetter(_letter);
      }

      unitOfWork.Commit();

      return _letter.ID;
    }
  }
}
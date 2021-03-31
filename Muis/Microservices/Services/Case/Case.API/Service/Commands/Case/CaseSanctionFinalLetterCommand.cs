using Case.API.Repository;
using Case.API.Services.Commands.Case.SubCommands;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseSanctionFinalLetterCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly Letter _letter;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseSanctionFinalLetterCommand(long caseID, Letter letter, Officer user, IEventBus eventBus)
    {
      _caseID = caseID;
      _letter = letter;
      _user = user;
      _eventBus = eventBus;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      if (_letter.ID == 0)
      {
        _letter.ID = await dbContext.Letter.InsertLetter(_letter);
      }
      else
      {
        await dbContext.Letter.UpdateLetter(_letter);
      }

      switch (_letter.Type)
      {
        case LetterType.Compound:
          await new SanctionCompositionSubCommand(
            dbContext,
            _caseID,
            _user,
            _letter.ID,
            _eventBus)
            .Invoke();
          break;

        case LetterType.Revocation:
          await new SanctionRevokeSubCommand(
            dbContext,
            _caseID,
            _user,
            _letter.ID,
            _eventBus)
            .Invoke();
          break;

        case LetterType.Suspension:
        case LetterType.ImmediateSuspension:
          await new SanctionSuspensionSubCommand(
            dbContext,
            _caseID,
            _letter.ID,
            _user,
            _eventBus)
            .Invoke();
          break;

        case LetterType.Warning:
          await new SanctionWarningSubCommand(
           dbContext,
           _caseID,
           _letter,
           _user,
           _eventBus)
           .Invoke();
          break;
      }

      unitOfWork.Commit();

      return _letter.ID;
    }
  }
}
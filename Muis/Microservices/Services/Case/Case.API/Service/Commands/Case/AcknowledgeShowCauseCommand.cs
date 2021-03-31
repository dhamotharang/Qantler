using Case.API.Params;
using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class AcknowledgeShowCauseCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly AcknowledgeShowCauseParam _data;
    readonly Officer _user;

    public AcknowledgeShowCauseCommand(long caseID, AcknowledgeShowCauseParam data, Officer user)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);
      @case.Sanction = _data.Sanction.Sanction;

      await dbContext.Case.UpdateCaseInfo(@case);

      _data.Sanction.ID = await dbContext.Sanction.InsertSanctionInfo(_data.Sanction);

      await dbContext.Case.MapCaseSanctionInfo(_caseID, _data.Sanction.ID);

      await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingFOC);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "AcknowledgeShowCause");
      var note = "";

      switch (_data.Sanction.Sanction)
      {
        case Sanction.Dismissed:
          note = _data.Note;
          break;

        case Sanction.Revocation:
          note = await dbContext.Translation.GetTranslation(Locale.EN, "RevocationSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Suspension:
          note = await dbContext.Translation.GetTranslation(Locale.EN, "SuspensionSanctionNote");
          note = string.Format(note, _data.Sanction.Value, _data.Note);
          break;

        case Sanction.Warning:
          note = await dbContext.Translation.GetTranslation(Locale.EN, "WarningSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Compound:
          note = await dbContext.Translation.GetTranslation(Locale.EN, "CompoundSanctionNote");
          note = string.Format(note, _data.Sanction.Value, _data.Note);
          break;
      }

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        Notes = note,
        User = _user
      }, _caseID);

      if (_data.Attachment?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachment)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          _activityID,
          _data.Attachment.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();

      return _data.Sanction.ID;
    }
  }
}
using Case.API.Params;
using Case.API.Repository;
using Case.API.Services.Commands.Case.SubCommands;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class FOCDecisionCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly FOCDecisionParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public FOCDecisionCommand(long caseID, FOCDecisionParam data, Officer user, IEventBus eventBus)
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

      _data.Sanction.Type = SanctionInfoType.Final;
      _data.Sanction.ID = await dbContext.Sanction.InsertSanctionInfo(_data.Sanction);

      await dbContext.Case.MapCaseSanctionInfo(_caseID, _data.Sanction.ID);

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "FOCDecision");

      var note = "";

      switch (_data.Sanction.Sanction)
      {
        case Sanction.Dismissed:
          await new DecisionDismissSubCommand(
            dbContext,
            _caseID,
            _eventBus)
            .Invoke();

          note = await dbContext.Translation.GetTranslation(Locale.EN, "DismissedSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Revocation:
          await new DecisionRevokeSubCommand(
            dbContext,
            _caseID)
            .Invoke();

          note = await dbContext.Translation.GetTranslation(Locale.EN, "RevocationSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Suspension:
          await new DecisionSuspensionSubCommand(
            dbContext,
            int.Parse(_data.Sanction.Value),
            _caseID)
            .Invoke();

          note = await dbContext.Translation.GetTranslation(Locale.EN, "SuspensionSanctionNote");
          note = string.Format(note, _data.Sanction.Value, _data.Note);
          break;

        case Sanction.ImmediateSuspension:
          await new DecisionImmediateSuspensionSubCommand(
            dbContext,
            _caseID)
            .Invoke();

          note = await dbContext.Translation.GetTranslation(Locale.EN, "ImmediateSuspensionSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Warning:
          await new DecisionWarningSubCommand(
            dbContext,
            _caseID)
            .Invoke();

          note = await dbContext.Translation.GetTranslation(Locale.EN, "WarningSanctionNote");
          note = string.Format(note, _data.Note);
          break;

        case Sanction.Compound:
          @case.Sanction = _data.Sanction.Sanction;
          @case.Status = CaseStatus.PendingSanctionLetter;
          @case.MinorStatus = null;
          await dbContext.Case.UpdateCaseInfo(@case);

          note = await dbContext.Translation.GetTranslation(Locale.EN, "CompoundSanctionNote");
          note = string.Format(note, _data.Sanction.Value, _data.Note);
          break;

        case Sanction.Reinstate:
          @case.Sanction = _data.Sanction.Sanction;
          @case.Status = @case.MinorStatus == CaseMinorStatus.CertificateCollected
            ? CaseStatus.ReinstateCertificate : CaseStatus.Closed;
          @case.MinorStatus = null;
          await dbContext.Case.UpdateCaseInfo(@case);

          note = await dbContext.Translation.GetTranslation(Locale.EN, "ReInitSanctionNote");
          note = string.Format(note, _data.Note);
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
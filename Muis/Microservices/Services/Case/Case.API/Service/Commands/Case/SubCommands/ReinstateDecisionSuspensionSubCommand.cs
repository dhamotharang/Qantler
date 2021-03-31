using Case.API.Params;
using Case.API.Repository;
using Case.Model;
using Core.Base;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class ReinstateDecisionSuspensionSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly ReinstateDecisionParam _data;
    readonly Officer _user;

    public ReinstateDecisionSuspensionSubCommand(DbContext dbContext,
      long caseID, Officer user, ReinstateDecisionParam data)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _user = user;
      _data = data;
    }
    public override async Task Execute()
    {
      var certificates = await _dbContext.Certificate.GetCertificate(_caseID);

      foreach (var certificate in certificates)
      {
        certificate.SuspendedUntil = certificate.SuspendedUntil.Value
          .AddMonths(int.Parse(_data.Sanction.Value));

        await _dbContext.Certificate.UpdateCertificate(certificate);
      }

      var note = await _dbContext.Translation.GetTranslation(Locale.EN, "ReinstateSuspensionNote");

      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Action = await _dbContext.Translation.GetTranslation(Locale.EN, "ReinstateSuspension"),
        User = _user,
        Notes = string.Format(note, _data.Sanction.Value, _data.Note),
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

      _data.Sanction.ID = await _dbContext.Sanction.InsertSanctionInfo(_data.Sanction);

      await _dbContext.Case.MapCaseSanctionInfo(_caseID, _data.Sanction.ID);

      var @case = await _dbContext.Case.GetBasicByID(_caseID);
      @case.Sanction = Sanction.Suspension;
      @case.Status = CaseStatus.PendingFOC;
      @case.MinorStatus = null;
      await _dbContext.Case.UpdateCaseInfo(@case);
    }
  }
}

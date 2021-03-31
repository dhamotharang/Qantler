using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.Base;
using Core.EventBus;
using Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class SanctionWarningSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly Officer _user;
    readonly Letter _letter;
    readonly IEventBus _eventBus;

    public SanctionWarningSubCommand(DbContext dbContext, long caseID,
      Letter letter, Officer user, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _user = user;
      _letter = letter;
      _eventBus = eventBus;
    }

    public override async Task Execute()
    {
      await _dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed);

      var lastActivity = (await _dbContext.Case.GetActivityByCaseID(_caseID))?.
      FirstOrDefault(x => x.Type == ActivityType.SanctionLetter);

      var draftAction = await _dbContext.Translation.GetTranslation(Locale.EN, "WarningDraftLetter");

      if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
      {
        await _dbContext.Activity.DeleteByID(lastActivity.ID);
      }

      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.SanctionLetter,
        Action = await _dbContext.Translation.GetTranslation(Locale.EN, "WarningFinalLetter"),
        User = _user
      }, _caseID);

      await _dbContext.Activity.MapActivityLetter(activityID, _letter.ID);

      var certificates = await _dbContext.Certificate.GetCertificate(_caseID);

      var oldSanctionInfo = (await _dbContext.Sanction.GetSanctionInfo(_caseID))
        .OrderBy(x => x.ID).Skip(1).FirstOrDefault();

      if (oldSanctionInfo.Sanction == Sanction.ImmediateSuspension)
      {
        foreach (var certificate in certificates)
        {
          var certOldStatus = certificate.Status;
          certificate.Status = CertificateStatus.Active;
          certificate.SuspendedUntil = null;

          await _dbContext.Certificate.UpdateCertificate(certificate);

          _eventBus.Publish(new OnCertificateStatusChangedEvent
          {
            NewStatus = CertificateStatus.Active,
            OldStatus = certOldStatus,
            Number = certificate.Number
          });
        }
      }
    }
  }
}

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
  public class SanctionRevokeSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly Officer _user;
    readonly long _letterID;
    readonly IEventBus _eventBus;

    public SanctionRevokeSubCommand(DbContext dbContext, long caseID,
      Officer user, long letterID, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _user = user;
      _letterID = letterID;
      _eventBus = eventBus;
    }
    public override async Task Execute()
    {
      await _dbContext.Case.UpdateStatus(_caseID, CaseStatus.CertificateCollection);

      var lastActivity = (await _dbContext.Case.GetActivityByCaseID(_caseID))?.
          FirstOrDefault(x => x.Type == ActivityType.SanctionLetter);

      var draftAction = await _dbContext.Translation.GetTranslation(Locale.EN, "RevocationDraftLetter");

      if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
      {
        await _dbContext.Activity.DeleteByID(lastActivity.ID);
      }

      var action = await _dbContext.Translation.GetTranslation(Locale.EN, "RevocationFinalLetter");

      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Type = ActivityType.SanctionLetter,
        Action = action,
        User = _user
      }, _caseID);

      await _dbContext.Activity.MapActivityLetter(activityID, _letterID);

      var certificates = await _dbContext.Certificate.GetCertificate(_caseID);

      foreach (var certificate in certificates)
      {
        var oldStatus = certificate.Status;
        certificate.Status = CertificateStatus.Revoked;
        await _dbContext.Certificate.UpdateCertificate(certificate);

        _eventBus.Publish(new OnCertificateStatusChangedEvent
        {
          NewStatus = CertificateStatus.Revoked,
          OldStatus = oldStatus,
          Number = certificate.Number
        });
      }
    }
  }
}

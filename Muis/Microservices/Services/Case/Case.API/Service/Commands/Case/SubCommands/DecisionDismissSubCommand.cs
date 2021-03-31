using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.Base;
using Core.EventBus;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class DecisionDismissSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly IEventBus _eventBus;

    public DecisionDismissSubCommand(DbContext dbContext, long caseID, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _eventBus = eventBus;
    }
    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);
      var oldStatus = @case.Status;
      var oldSanction = @case.Sanction;
      @case.Sanction = null;
      @case.Status = CaseStatus.Dismissed;
      @case.MinorStatus = null;
      await _dbContext.Case.UpdateCaseInfo(@case);

      var certificates = await _dbContext.Certificate.GetCertificate(_caseID);

      if (oldSanction == Sanction.ImmediateSuspension)
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

      var premises = await _dbContext.Premise.GetPremises(_caseID);

      _eventBus.Publish(new OnCaseStatusChangedEvent
      {
        NewStatus = CaseStatus.Dismissed,
        OldStatus = oldStatus,
        PremisesID = premises.Select(x => x.ID).ToList()
      });
    }
  }
}

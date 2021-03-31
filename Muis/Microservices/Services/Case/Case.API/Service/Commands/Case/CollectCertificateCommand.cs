using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CollectCertificateCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly Attachment _attachment;
    readonly Officer _user;
    readonly IEventBus _eventBus;
    readonly string _senderName;

    public CollectCertificateCommand(long caseID, Attachment attachment, string senderName, Officer user, IEventBus eventBus)
    {
      _caseID = caseID;
      _attachment = attachment;
      _user = user;
      _senderName = senderName;
      _eventBus = eventBus;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var @case = await dbContext.Case.GetBasicByID(_caseID);

      var premises = await dbContext.Premise.GetPremises(_caseID);

      var certificates = await dbContext.Certificate.GetCertificate(_caseID);

      if ((@case.Sanction == Sanction.Revocation
        || ((@case.Sanction == Sanction.Suspension
        || @case.Sanction == Sanction.ImmediateSuspension)
        && certificates.Any(x => x.SuspendedUntil > x.ExpiresOn)))
        && @case.OtherStatus == null)
      {
        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed);

        _eventBus.Publish(new OnCaseStatusChangedEvent
        {
          NewStatus = CaseStatus.Closed,
          OldStatus = @case.Status,
          PremisesID = premises.Select(x => x.ID).ToList()
        });
      }
      else if ((@case.Sanction == Sanction.Revocation
        || ((@case.Sanction == Sanction.Suspension
        || @case.Sanction == Sanction.ImmediateSuspension)
        && certificates.Any(x => x.SuspendedUntil > x.ExpiresOn)))
        && @case.OtherStatus != null)
      {
        await dbContext.Case.UpdateStatus(_caseID, @case.Status, CaseMinorStatus.CertificateCollected);
      }
      else
      {
        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingReinstateInspection);
      }

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "CertifcateReceived");

      var note = await dbContext.Translation.GetTranslation(Locale.EN, "CertifcateReceivedNote");

      var offender = await dbContext.Offender.GetOffenderByID(@case.OffenderID.Value);

      var sanctionName = "";
      if (@case.Sanction == Sanction.Suspension
        || @case.Sanction == Sanction.ImmediateSuspension)
      {
        sanctionName = "suspension";
      }
      else
      {
        sanctionName = "revocation";
      }

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        User = _user,
        Notes = string.Format(note, _senderName, offender.Name, sanctionName)
      }, _caseID);

      _attachment.ID = await dbContext.Attachment.InsertAttachment(_attachment);

      await dbContext.Activity.MapActivityAttachments(
        _activityID,
        _attachment.ID);

      unitOfWork.Commit();

      return _activityID;
    }
  }
}
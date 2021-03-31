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
  public class CaseReinstateCertificateCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly Attachment _attachment;
    readonly Officer _user;
    readonly IEventBus _eventBus;
    readonly string _senderName;

    public CaseReinstateCertificateCommand(long caseID, Attachment attachment, string senderName, Officer user, IEventBus eventBus)
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

      await dbContext.Case.UpdateStatus(_caseID, CaseStatus.Closed, @case.MinorStatus);

      _eventBus.Publish(new OnCaseStatusChangedEvent
      {
        NewStatus = CaseStatus.Closed,
        OldStatus = @case.Status,
        PremisesID = premises.Select(x => x.ID).ToList()
      });

      foreach (var certificate in certificates)
      {
        var certOldStatus = certificate.Status;
        certificate.Status = CertificateStatus.Active;
        await dbContext.Certificate.UpdateCertificate(certificate);

        _eventBus.Publish(new OnCertificateStatusChangedEvent
        {
          NewStatus = CertificateStatus.Active,
          OldStatus = certOldStatus,
          Number = certificate.Number
        });
      }

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "CertifcateReinstated");

      var note = await dbContext.Translation.GetTranslation(Locale.EN, "CertifcateReinstateNote");

      var offender = await dbContext.Offender.GetOffenderByID(@case.OffenderID.Value);

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        User = _user,
        Notes = string.Format(note, _senderName, offender.Name
          , certificates.First().SuspendedUntil.Value.ToString("MMM d, yyyy"))
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
using Case.API.Params;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class CaseImmediateSuspensionCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly ImmediateSuspensionParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public CaseImmediateSuspensionCommand(long caseID, ImmediateSuspensionParam data,
      Officer user, IEventBus eventBus)
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

      var @case = await dbContext.Case.GetBasicByID(_caseID);
      @case.Status = CaseStatus.PendingFOC;
      @case.Sanction = Sanction.ImmediateSuspension;
      @case.SuspendedFrom = DateTime.UtcNow;
      await dbContext.Case.UpdateCaseInfo(@case);

      var sanction = new SanctionInfo
      {
        CaseID = _caseID,
        Sanction = Sanction.ImmediateSuspension,
        Type = SanctionInfoType.Recommended
      };

      sanction.ID = await dbContext.Sanction.InsertSanctionInfo(sanction);

      await dbContext.Case.MapCaseSanctionInfo(_caseID, sanction.ID);

      var certificates = await dbContext.Certificate.GetCertificate(_caseID);

      foreach (var certificate in certificates)
      {
        var certOldStatus = certificate.Status;
        certificate.Status = CertificateStatus.Suspended;
        certificate.SuspendedUntil = @case.SuspendedFrom.Value.AddMonths(Int32.Parse(_data.Months));

        await dbContext.Certificate.UpdateCertificate(certificate);

        _eventBus.Publish(new OnCertificateStatusChangedEvent
        {
          NewStatus = CertificateStatus.Suspended,
          OldStatus = certOldStatus,
          Number = certificate.Number
        });
      }

      var action = await dbContext.Translation.GetTranslation(Locale.EN, "CaseImmediateSuspension");

      var _activityID = await dbContext.Activity.InsertActivity(new Activity
      {
        Action = action,
        User = _user,
        Notes = _data.Notes
      }, _caseID);

      if (_data.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _data.Attachments)
        {
          attachment.ID = await dbContext.Attachment.InsertAttachment(attachment);
        }

        await dbContext.Activity.MapActivityAttachments(
          _activityID,
          _data.Attachments.Select(e => e.ID).ToArray());
      }

      unitOfWork.Commit();

      return _activityID;
    }
  }
}
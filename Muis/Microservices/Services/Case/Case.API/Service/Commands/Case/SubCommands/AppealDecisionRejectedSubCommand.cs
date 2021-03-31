using System;
using System.Linq;
using System.Threading.Tasks;
using Case.API.Models;
using Case.API.Repository;
using Case.Events;
using Case.Model;
using Core.Base;
using Core.EventBus;
using Core.Model;

namespace Case.API.Service.Commands.Case.SubCommands
{
  public class AppealDecisionRejectedSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly AppealDecisionParam _param;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public AppealDecisionRejectedSubCommand(DbContext dbContext, long caseID,
      AppealDecisionParam param, Officer user, IEventBus eventBus)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _param = param;
      _user = user;
      _eventBus = eventBus;
    }

    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);

      var oldStatus = @case.Status;
      @case.Status = CaseStatus.CertificateCollection;
      @case.MinorStatus = null;
      @case.OtherStatus = null;
      @case.OtherStatusMinor = null;

      await _dbContext.Case.UpdateCaseInfo(@case);

      // Insert activity
      var activityID = await _dbContext.Activity.InsertActivity(new Activity
      {
        Action = await _dbContext.Translation.GetTranslation(Locale.EN, "AppealDecision"),
        Notes = string.Format(
          await _dbContext.Translation.GetTranslation(Locale.EN, "AppealRejectedNote"),
          _param.SanctionInfo.Value,
          _param.Notes),
        User = _user
      }, _caseID);

      if (_param.Attachments?.Any() ?? false)
      {
        foreach (var attachment in _param.Attachments)
        {
          attachment.ID = await _dbContext.Attachment.InsertAttachment(attachment);
        }

        await _dbContext.Activity.MapActivityAttachments(
          activityID,
          _param.Attachments.Select(e => e.ID).ToArray());
      }

      var premises = await _dbContext.Premise.GetPremises(_caseID);

      _eventBus.Publish(new OnCaseStatusChangedEvent
      {
        NewStatus = @case.Status,
        OldStatus = oldStatus,
        PremisesID = premises.Select(x => x.ID).ToList()
      });
    }
  }
}

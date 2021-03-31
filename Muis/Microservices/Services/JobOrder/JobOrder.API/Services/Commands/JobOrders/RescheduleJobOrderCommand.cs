using Core.API;
using Core.API.Repository;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Helpers;
using JobOrder.API.Models;
using JobOrder.API.Repository;
using JobOrder.Events;
using System.Threading.Tasks;
using JobOrder.Model;
using System.Linq;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class RescheduleJobOrderCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly RescheduleParam _param;

    readonly Officer _user;

    readonly IEventBus _eventBus;

    readonly IEmailService _emailService;

    readonly ISmtpProvider _smtpProvider;

    public RescheduleJobOrderCommand(long id, RescheduleParam param, Officer user,
      IEventBus eventBus, IEmailService emailService, ISmtpProvider smtpProvider)
    {
      _id = id;
      _param = param;

      _user = user;

      _eventBus = eventBus;

      _emailService = emailService;

      _smtpProvider = smtpProvider;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var jobOrder = await dbContext.JobOrder.GetJobOrderByID(_id);
      var oldScheduledOn = jobOrder.ScheduledOn.Value;
      var oldScheduledOnTo = jobOrder.ScheduledOnTo.Value;

      jobOrder.ScheduledOn = _param.ScheduledOn;
      jobOrder.ScheduledOnTo = _param.ScheduledOnTo;

      await dbContext.JobOrder.UpdateJobOrder(jobOrder);

      await dbContext.JobOrder.InsertRescheduleHistory(_id, _param.Reason.ID, _param.Notes);

      // TODO Reconsider timezone. As of now, assumes timezone in SGT (+8:00)
      var oldScheduledOnText = oldScheduledOn.AddHours(8).ToString("dd MMM yyyy");
      var oldScheduledOnToText = oldScheduledOnTo.AddHours(8).ToString("dd MMM yyyy");

      var scheduledOnText = _param.ScheduledOn.Value.AddHours(8).ToString("dd MMM yyyy");
      var scheduledOnToText = _param.ScheduledOnTo.Value.AddHours(8).ToString("dd MMM yyyy");

      var logText = string.Format(
        await dbContext.Translation.GetTranslation(Locale.EN, "RescheduledInspection"),
        $"{oldScheduledOnText} to {oldScheduledOnToText}",
        $"{scheduledOnText} to {scheduledOnToText}");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.JobOrder,
        Notes = $"{_param.Reason.Value}\n{_param.Notes}",
        Action = logText,
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.JobOrder.MapLog(_id, logID);

      _eventBus.Publish(new OnJobOrderRescheduledEvent
      {
        ID = _id,
        RefID = jobOrder.RefID,
        Type = jobOrder.Type.Value,
        OldScheduledOn = oldScheduledOn,
        NewScheduledOn = _param.ScheduledOn.Value
      });

      await SendEmail(dbContext, jobOrder);

      uow.Commit();

      return Unit.Default;
    }

    public async Task SendEmail(DbContext dbContext, JobOrder.Model.JobOrder jobOrder)
    {
      if (jobOrder.Type != Model.JobOrderType.Audit)
      {
        return;
      }

      var emailID = (await dbContext.JobOrder.GetEmails(jobOrder.ID)).FirstOrDefault();
      if (emailID == 0)
      {
        return;
      }

      var email = await _emailService.GetByID(emailID);

      email = await EmailHelper.GenerateAuditInspectionEmail(jobOrder,
        _emailService,
        _user,
        EmailTemplateType.RescheduleAuditInspection,
        email.To);

      email = await _emailService.Save(email);

      await dbContext.JobOrder.MapEmail(jobOrder.ID, email.ID);

      _smtpProvider.Send(new Mail
      {
        From = email.From,
        Recipients = email.To?.Split(";"),
        Cc = email.Cc?.Split(";"),
        Bcc = email.Bcc?.Split(";"),
        Subject = email.Title,
        Body = email.Body,
        IsBodyHtml = email.IsBodyHtml
      });
    }
  }
}

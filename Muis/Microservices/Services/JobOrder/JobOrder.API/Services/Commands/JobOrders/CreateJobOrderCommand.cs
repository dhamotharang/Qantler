using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Helpers;
using JobOrder.API.Models;
using JobOrder.API.Repository;
using JobOrder.Events;
using JobOrder.Model;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class CreateJobOrderCommand : IUnitOfWorkCommand<Model.JobOrder>
  {
    readonly ScheduleJobOrderParam _param;
    readonly Guid _userID;
    readonly string _userName;
    readonly string _userEmail;

    readonly IEventBus _eventBus;

    readonly IEmailService _emailService;

    readonly ISmtpProvider _smtpProvider;

    public CreateJobOrderCommand(ScheduleJobOrderParam param, Guid userID, string userName,
     string userEmail, IEventBus eventBus, IEmailService emailService, ISmtpProvider smtpProvider)
    {
      _param = param;
      _userID = userID;
      _userName = userName;
      _userEmail = userEmail;

      _eventBus = eventBus;

      _emailService = emailService;

      _smtpProvider = smtpProvider;
    }

    public async Task<Model.JobOrder> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      if (_param.ContactPerson != null)
      {
        var person = await dbContext.Person.GetPersonByID(_param.ContactPerson.ID);
        if (person == null)
        {
          await dbContext.Person.InsertPerson(_param.ContactPerson);
        }

        if (_param.ContactPerson.ContactInfos != null)
        {         
          foreach (ContactInfo item in _param.ContactPerson.ContactInfos)
          {
            bool isExists = false;

            var contactInfos = await dbContext.ContactInfo.Select(_param.ContactPerson.ID, 
              item.Type);

            if (contactInfos != null && contactInfos.Count > 0)
            {
              isExists = true;

              await dbContext.ContactInfo.UpdateContactInfo(item);

              break;
            }

            if (!isExists)
            {
              var contactInfoID = await dbContext.ContactInfo.InsertContactInfo(item);

              await dbContext.Person.MapContactInfoToPerson(
                _param.ContactPerson.ID, contactInfoID);
            }
          }
        }
      }

      var id = await dbContext.JobOrder.InsertJobOrder(_param);

      var logText = await dbContext.Translation.GetTranslation(Locale.EN, "JobOrderCreate");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.JobOrder,
        Action = logText,
        UserID = _userID,
        UserName = _userName
      });

      var result = await dbContext.JobOrder.GetJobOrderByID(id);


      _eventBus.Publish(new OnJobOrderCreatedEvent
      {
        ID = result.ID,
        RefID = result.RefID,
        ScheduledOn = result.ScheduledOn,
        Type = result.Type
      });

      if (_param.Type == Model.JobOrderType.Audit
        && _param.Email != null)
      {
        var email = await EmailHelper.GenerateAuditInspectionEmail(result,
          _emailService,
          new Officer(_userID, _userName, _userEmail),
          EmailTemplateType.AuditInspection,
          _param.Email);

        email = await _emailService.Save(email);

        await dbContext.JobOrder.MapEmail(id, email.ID);

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

      uow.Commit();

      return result;
    }
  }
}

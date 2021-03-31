using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.API.Smtp;
using Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class OnShowCauseLetterCommand : IUnitOfWorkCommand<long>
  {
    readonly Letter _letter;
    readonly long _caseID;
    readonly Officer _user;
    readonly ISmtpProvider _smtpProvider;

    public OnShowCauseLetterCommand(Letter letter, long caseID, Officer user, ISmtpProvider smtpProvider)
    {
      _letter = letter;
      _caseID = caseID;
      _user = user;
      _smtpProvider = smtpProvider;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var action = "";
      var activityID = 0L;
      var @case = await dbContext.Case.GetBasicByID(_caseID);
      _letter.Type = LetterType.ShowCause;

      if (_letter.Status == LetterStatus.Draft && _letter.ID == 0)
      {
        action = await dbContext.Translation.GetTranslation(Locale.EN, "LetterStatusDraft");

        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingShowCause);

        activityID = await dbContext.Activity.InsertActivity(new Activity
        {
          Type = ActivityType.ShowCauseLetter,
          Action = action,
          User = _user
        }, _caseID);
      }
      else if (_letter.Status == LetterStatus.Final)
      {
        var lastActivity = (await dbContext.Case.GetActivityByCaseID(_caseID))?.
            FirstOrDefault(x => x.Type == ActivityType.ShowCauseLetter);

        var draftAction = await dbContext.Translation.GetTranslation(Locale.EN, "LetterStatusDraft");

        if (lastActivity != null && lastActivity.Action.Equals(draftAction, StringComparison.CurrentCultureIgnoreCase))
        {
          await dbContext.Activity.DeleteByID(lastActivity.ID);
        }

        action = await dbContext.Translation.GetTranslation(Locale.EN, "LetterStatusFinal");

        await dbContext.Case.UpdateStatus(_caseID, CaseStatus.PendingAcknowledgement);

        activityID = await dbContext.Activity.InsertActivity(new Activity
        {
          Type = ActivityType.ShowCauseLetter,
          Action = action,
          User = _user
        }, _caseID);
      }

      if (_letter.ID == 0)
      {
        _letter.ID = await dbContext.Letter.InsertLetter(_letter);
        await dbContext.Activity.MapActivityLetter(activityID, _letter.ID);
      }
      else if (_letter.Status == LetterStatus.Final)
      {
        await dbContext.Letter.UpdateLetter(_letter);
        await dbContext.Activity.MapActivityLetter(activityID, _letter.ID);
      }
      else
      {
        await dbContext.Letter.UpdateLetter(_letter);
      }

      _smtpProvider.Send(new Mail
      {
        From = "israel.ravi@qantler.com",
        Recipients = new string[] { "israel.ravi@qantler.com" },
        Cc = null,
        Bcc = null,
        Subject = "Test",
        Body = "Test",
        IsBodyHtml = true,
        Attachments = _letter.Email.Attachments?.Select(e => new MailAttachment
        {
          Type = MailAttachmentType.FILE,
          Data = e.Data,
          Name = e.Name
        })?.ToList()
      });

      unitOfWork.Commit();

      return _letter.ID;
    }
  }
}
using Core.API;
using Core.API.Repository;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class ApproveRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Review> _reviews;
    readonly IEmailService _emailService;

    readonly IEventBus _eventBus;

    readonly ISmtpProvider _smtpProvider;

    public ApproveRequestCommand(IList<Review> reviews, IEventBus eventBus,
      IEmailService emailService, ISmtpProvider smtpProvider)
    {
      _reviews = reviews;

      _emailService = emailService;

      _eventBus = eventBus;

      _smtpProvider = smtpProvider;
    }
    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      foreach (var review in _reviews)
      {
        var request = await dbContext.Request.GetRequestByIDBasic(review.RequestID);
        if (request.Status >= RequestStatus.Approved)
        {
          continue;
        }

        if (review.Email != null)
        {
          var email = await _emailService.Save(review.Email);
          review.EmailID = email.ID;
        }

        switch (request.Type)
        {
          case RequestType.HC02:
            var isFoodManufacturer = request.LineItems
              .FirstOrDefault(e => e.Scheme == Scheme.FoodManufacturing) != null;
            if (isFoodManufacturer)
            {
              await new HC02FMApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            }
            else
            {
              await new HC02ApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            }
            break;
          case RequestType.HC03:
            await new HC03ApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            break;
          case RequestType.HC04:
            await new HC04ApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            break;
          case RequestType.New:
            await new NewApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            break;
          case RequestType.Renewal:
            await new RenewalApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            break;
          case RequestType.Legacy:
            await new LegacyApprovalProcessCommand(request, review, _eventBus, dbContext).Invoke();
            break;
        }
      }

      var rejectEmail = _reviews.FirstOrDefault(e => e.Email != null)?.Email;

      if (rejectEmail != null)
      {
        _smtpProvider.Send(new Mail
        {
          From = rejectEmail.From,
          Recipients = rejectEmail.To?.Split(";"),
          Cc = rejectEmail.Cc?.Split(";"),
          Bcc = rejectEmail.Bcc?.Split(";"),
          Subject = rejectEmail.Title,
          Body = rejectEmail.Body,
          IsBodyHtml = rejectEmail.IsBodyHtml,
          Attachments = rejectEmail.Attachments?.Select(e => new MailAttachment
          {
            Type = MailAttachmentType.CID,
            Key = e.Key,
            ID = e.ID,
            Data = e.Data
          })?.ToList()
        });
      }

      unitOfWork.Commit();

      return Unit.Default;
    }   
  }
}

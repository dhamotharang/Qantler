using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.Events;
using Request.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class RecommendRequestWithRejectionCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Review> _reviews;

    readonly IEmailService _emailService;

    readonly IEventBus _eventBus;

    readonly Officer _assignedTo;

    public RecommendRequestWithRejectionCommand(IList<Review> reviews, IEventBus eventBus,
      IEmailService emailService, Officer assignedTo)
    {
      _reviews = reviews;

      _emailService = emailService;

      _eventBus = eventBus;

      _assignedTo = assignedTo;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await new RequestRecommendValidator(_reviews, dbContext).Invoke();

      foreach (var review in _reviews)
      {
        var request = await dbContext.Request.GetRequestByIDBasic(review.RequestID);

        var oldStatus = request.Status;

        if (review.Email != null)
        {
          var email = await _emailService.Save(review.Email);
          review.EmailID = email.ID;
        }

        await dbContext.Request.InsertReview(review);

        var logText = await dbContext.Transalation.GetTranslation(Locale.EN, 
          "RecommendReviewApproval");

        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = logText,
          UserID = review.ReviewerID.Value,
          UserName = review.ReviewerName,
        });

        await dbContext.Request.MapLog(review.RequestID, logID);

        await dbContext.Request.SetAssignedOfficer(review.RequestID,
             _assignedTo.ID, _assignedTo.Name);

        await dbContext.Request.UpdateStatus(review.RequestID,
          RequestStatus.PendingReviewApproval, null);

        await dbContext.Request.InsertActionHistory(new RequestActionHistory
        {
          Action = RequestActionType.Review,
          RequestID = review.RequestID,
          Officer = new Officer
          {
            ID = review.ReviewerID.Value,
            Name = review.ReviewerName
          }
        });

        _eventBus.Publish(new OnRequestStatusChangedEvent
        {
          ID = request.ID,
          RefID = request.RefID,
          OldStatus = oldStatus,
          NewStatus = RequestStatus.PendingReviewApproval
        });

        _eventBus.Publish(new SendNotificationEvent
        {
          Title = await dbContext.Transalation.GetTranslation(Locale.EN,
            "RequestForReviewApprovalNotifTitle"),
          Body = string.Format(await dbContext.Transalation.GetTranslation(Locale.EN,
            "RequestForReviewApprovalNotifBody"),
            review.ReviewerName,
            request.CustomerName),
          Module = "Request",
          RefID = $"{request.ID}",
          To = new string[] { _assignedTo.ID.ToString() }
        });

      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

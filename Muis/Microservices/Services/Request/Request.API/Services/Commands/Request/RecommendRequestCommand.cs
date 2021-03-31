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
  public class RecommendRequestCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Review> _reviews;

    readonly IEventBus _eventBus;

    public RecommendRequestCommand(IList<Review> reviews, IEventBus eventBus)
    {
      _reviews = reviews;

      _eventBus = eventBus;
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

        await dbContext.Request.InsertReview(review);

        var logText = await dbContext.Transalation.GetTranslation(Locale.EN, "RecommendRequest");

        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = logText,
          UserID = review.ReviewerID.Value,
          UserName = review.ReviewerName,
        });

        await dbContext.Request.MapLog(review.RequestID, logID);

        await dbContext.Request.SetAssignedOfficer(review.RequestID, null, null);

        await dbContext.Request.UpdateStatus(review.RequestID,
          RequestStatus.PendingApproval, null);

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
          NewStatus = RequestStatus.PendingApproval
        });

        _eventBus.Publish(new SendNotificationWithPermissionsEvent
        {
          Title = await dbContext.Transalation.GetTranslation(Locale.EN,
            "RequestForApprovalNotifTitle"),
          Body = string.Format(await dbContext.Transalation.GetTranslation(Locale.EN,
            "RequestForApprovalNotifBody"),
            review.ReviewerName,
            request.CustomerName),
          Module = "Request",
          RefID = $"{request.ID}",
          RequestTypes = new int[] { (int)request.Type },
          Permissions = new Permission[] { Permission.RequestApprove },
          Excludes = new string[] { review.ReviewerID.Value.ToString() }
        });
      }

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}

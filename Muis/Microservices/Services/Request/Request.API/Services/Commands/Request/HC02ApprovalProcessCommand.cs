using Core.API.Exceptions;
using Core.EventBus;
using Core.Model;
using Request.API.Repository;
using Request.API.Services.Commands.Request.SubCommands;
using Request.API.Services.Commands.Request.Validators;
using Request.API.Strategies.Certificate360;
using Request.Events;
using Request.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class HC02ApprovalProcessCommand
  {
    readonly Review _review;
    readonly Model.Request _requestBasic;
    readonly IEventBus _eventBus;

    readonly DbContext _dbContext;

    public HC02ApprovalProcessCommand(Model.Request requestBasic, Review review,
      IEventBus eventBus, DbContext dbContext)
    {
      _requestBasic = requestBasic;
      _review = review;

      _eventBus = eventBus;

      _dbContext = dbContext;
    }

    public async Task Invoke()
    {
      await new ApprovalPolicyValidator(_review, _dbContext)
        .Next(new HC02ApprovalValidator(_review, _dbContext))
        .Next(new AmmendCertificateValidator(_review, _dbContext))
        .Invoke();

      var oldStatus = _requestBasic.Status;

      var approvedLineItems = _review.LineItems.Where(e => e.Approved.Value).ToList();
      var approved = approvedLineItems.Count() >= 1;
      RequestStatus newStatus = approved ? RequestStatus.Closed : RequestStatus.Rejected;

      if (approved && _requestBasic.CodeID == null)
      {
        throw new BadRequestException(
          await _dbContext.Transalation.GetTranslation(Locale.EN, "RequestApproveNoCode"));
      }

      await _dbContext.Request.InsertReview(_review);

      var logText = await _dbContext.Transalation.GetTranslation(Locale.EN,
          approved ? "RequestApproved" : "RequestRejected");

      var logID = await _dbContext.Log.InsertLog(new Log
      {
        Action = logText,
        UserID = _review.ReviewerID.Value,
        UserName = _review.ReviewerName,
      });

      await _dbContext.Request.MapLog(_review.RequestID, logID);

      await _dbContext.Request.InsertActionHistory(new RequestActionHistory
      {
        Action = RequestActionType.Approved,
        RequestID = _review.RequestID,
        Officer = new Officer
        {
          ID = _review.ReviewerID.Value,
          Name = _review.ReviewerName
        }
      });

      await _dbContext.Request.UpdateStatus(_review.RequestID, newStatus, null);

      _eventBus.Publish(new OnRequestStatusChangedEvent
      {
        ID = _requestBasic.ID,
        RefID = _requestBasic.RefID,
        OldStatus = oldStatus,
        NewStatus = newStatus
      });

      await UpdateCertificate360(approvedLineItems, new Officer(_review.ReviewerID.Value,
        _review.ReviewerName));

      await new SyncNewIngredientSubCommand(
          _review,
          _dbContext,
          _eventBus)
        .Invoke();
    }

    async Task UpdateCertificate360(IList<ReviewLineItem> approvedLineItems, Officer officer)
    {
      foreach (var item in approvedLineItems)
      {
        var request = await _dbContext.Request.GetRequestByID(_requestBasic.ID);

        var lineItem = request.LineItems.FirstOrDefault(e => e.Scheme == item.Scheme);

        var certificateNo = lineItem?.Characteristics?.FirstOrDefault(e =>
          e.Type == RequestCharType.IssuedCertificate)?.Value;

        if (string.IsNullOrEmpty(certificateNo))
        {
          throw new BadRequestException(
            await _dbContext.Transalation.GetTranslation(Locale.EN, "RequestApproveNoCertificate"));
        }

        await new HC02Certificate360Strategy(_dbContext, request, certificateNo, officer)
           .Invoke();
      }
    }
  }
}
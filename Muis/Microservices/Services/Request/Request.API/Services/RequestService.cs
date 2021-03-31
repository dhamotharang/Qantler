using System;
using System.Collections.Generic;
using Request.API.Services;
using System.Threading.Tasks;
using Request.API.Params;
using Request.API.Services.Commands.Request;
using Request.Model;
using Core.EventBus;
using Core.API.Smtp;
using Core.Model;
using Core.API;
using Core.API.Provider;
using Request.API.DTO;
using System.Linq;

namespace Request.Service
{
  public class RequestService : TransactionalService,
                                IRequestService
  {
    readonly IEmailService _emailService;

    readonly IEventBus _eventBus;
    readonly ISmtpProvider _smtpProvider;

    public RequestService(IDbConnectionProvider connectionProvider, IEmailService emailService,
      IEventBus eventBus, ISmtpProvider smtpProvider)
         : base(connectionProvider)
    {
      _emailService = emailService;

      _eventBus = eventBus;

      _smtpProvider = smtpProvider;
    }

    public async Task<IEnumerable<Model.Request>> QueryRequest(RequestOptions options)
    {
      return await Execute(new QueryRequestCommand(options));
    }

    public async Task<Model.Request> GetRequestByIDBasic(long id)
    {
      return await Execute(new GetRequestByIDBasicCommand(id));
    }

    public async Task<IEnumerable<Model.Request>> GetRelatedRequest(long id)
    {
      return await Execute(new GetRelatedRequestCommand(id));
    }

    public async Task<Model.Request> GetRequestByID(long id)
    {
      return await Execute(new GetRequestByIDCommand(id));
    }

    public async Task<Model.Request> GetRequestByRefID(string refId)
    {
      return await Execute(new GetRequestByRefIDCommand(refId));
    }

    public async Task<Model.Request> SubmitRequest(Model.Request request)
    {
      return await Execute(new SubmitRequestCommand(request, _eventBus));
    }

    public async Task<Model.Request> UpdateRequest(Model.Request request)
    {
      return await Execute(new UpdateRequestCommand(request));
    }

    public async Task EscalateRequest(long requestID, EscalateStatus status, string remarks,
      Guid userID, string userName)
    {
      await Execute(new EscalateRequestCommand(requestID, status, remarks, userID, userName)
        );
    }

    public async Task KIV(long id, string notes, DateTimeOffset remindOn, Guid userID,
      string userName)
    {
      await Execute(new KIVRequestCommand(id, notes, remindOn, userID, userName));
    }

    public async Task RevertKIV(long id, Guid userID, string userName)
    {
      await Execute(new RevertKIVCommand(id, userID, userName));
    }

    public async Task ScheduledInspection(long id, long jobID, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes, Guid userID, string userName)
    {
      await Execute(new ScheduledInspectionCommand(id, jobID, scheduledOn, scheduledOnTo,
        notes, userID, userName));
    }

    public async Task Review(RecommendParam recommendParam)
    {
      await Execute(new ReviewRequestCommand(recommendParam.reviews,
          _eventBus, _emailService));
    }

    public async Task Recommend(RecommendParam recommendParam)
    {
      var hasRejectedItems = recommendParam.reviews
        .Any(e => e.LineItems.Any(li => !li.Approved.Value));

      if (hasRejectedItems)
      {
        await Execute(new RecommendRequestWithRejectionCommand(recommendParam.reviews,
          _eventBus, _emailService, recommendParam.AssignedTo));
      }
      else
      {
        await Execute(new RecommendRequestCommand(recommendParam.reviews, _eventBus));
      }
    }

    public async Task Approve(IList<Review> reviews)
    {
      await Execute(new ApproveRequestCommand(reviews, _eventBus, _emailService, _smtpProvider));
    }

    public async Task Reaudit(long id, string notes, Guid userID, string userName)
    {
      await Execute(new ReauditRequestCommand(id, notes, userID, userName, _eventBus));
    }

    public async Task<IList<Review>> GetReviews(long[] requestIDs)
    {
      return await Execute(new QueryReviewCommand(requestIDs));
    }

    public async Task Reassign(long requestID, Officer toOfficer, string notes, Officer user)
    {
      await Execute(new ReassignRequestCommand(requestID, toOfficer, notes, user, _eventBus));
    }

    public async Task OnBillGeneratedForRequest(long requestID, long billID)
    {
      await Execute(new OnBillGeneratedForRequestCommand(requestID, _eventBus));
    }

    public async Task OnBillPaid(long requestID, long billID)
    {
      await Execute(new OnBillPaidCommand(requestID, _eventBus));
    }

    public async Task UpdateRequestStatus(long requestID, RequestStatus status,
      RequestStatusMinor? statusMinor, Officer user)
    {
      await Execute(new UpdateRequestStatusCommand(requestID, status, statusMinor, user));
    }

    public async Task<Model.Request> ValidateRequest(Scheme? Scheme, SubScheme? SubScheme, Premise premise)
    {
      return await Execute(new ValidateRequestCommand(Scheme, SubScheme, premise));
    }

    public async Task ProceedForReview(long id, Officer officer)
    {
      await Execute(new ProceedForReviewCommand(id, officer, _eventBus));
    }

    public async Task ReinstateRequest(long id, string notes, Officer user, Master reason)
    {
      await Execute(new ReinstateRequestCommand(id, notes, user, reason, _eventBus));
    }

  }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.DTO;
using Request.API.Params;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RequestController : ControllerBase
  {
    readonly IRequestService _requestservice;

    public RequestController(IRequestService requestService)
    {
      _requestservice = requestService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IEnumerable<Model.Request>> Query(
      long? id = null,
      Guid? customerId = null,
      string customerCode = null,
      string customer = null,
      string premise = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      RFAStatus? rfaStatus = null,
      EscalateStatus? escalateStatus = null,
      [FromQuery] RequestStatus[] status = null,
      [FromQuery] RequestType[] type = null,
      [FromQuery] Guid[] assignedTo = null,
      [FromQuery] RequestStatusMinor[] statusMinor = null)
    {
      return await _requestservice.QueryRequest(new RequestOptions
      {
        ID = id,
        CustomerID = customerId,
        CustomerCode = customerCode,
        Customer = customer,
        Premise = premise,
        Status = status,
        Type = type,
        From = from,
        To = to,
        RFAStatus = rfaStatus,
        AssignedTo = assignedTo,
        EscalateStatus = escalateStatus,
        StatusMinor = statusMinor
      });
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Model.Request> Get(long id)
    {
      return await _requestservice.GetRequestByID(id);
    }

    [HttpGet]
    [Route("{id:int}/basic")]
    public async Task<Model.Request> GetRequestByIDBasic(long id)
    {
      return await _requestservice.GetRequestByIDBasic(id);
    }

    [HttpGet]
    [Route("{id:int}/related")]
    public async Task<IEnumerable<Model.Request>> GetRelatedRequest(long id)
    {
      return await _requestservice.GetRelatedRequest(id);
    }

    [HttpGet]
    [Route("CheckRequest")]
    public async Task<Model.Request> GetRequestByID(string refId)
    {
      return await _requestservice.GetRequestByRefID(refId);
    }

    [HttpPost]
    [Route("")]
    public async Task<Model.Request> Post([FromBody] Model.Request request)
    {
      return await _requestservice.SubmitRequest(request);
    }

    [HttpPost]
    [Route("{id:int}/escalate")]
    public async Task<string> Escalate(long id, EscalateStatus status, string remarks,
      Guid userID, string userName)
    {
      await _requestservice.EscalateRequest(id, status, remarks, userID, userName);
      return "Ok";
    }

    [HttpPost]
    [Route("{rfaid:int}/requpdate")]
    public async Task<Model.Request> Post([FromBody] Model.Request request, long rfaid, Guid userID,
      string userName)
    {
      return await _requestservice.UpdateRequest(request);
    }

    [HttpPost]
    [Route("{id:int}/kiv")]
    public async Task<string> KIV(long id, DateTimeOffset remindOn, Guid userID, string userName,
      string notes = null)
    {
      await _requestservice.KIV(id, notes, remindOn, userID, userName);
      return "OK";
    }

    [HttpDelete]
    [Route("{id:int}/kiv")]
    public async Task<string> RevertKIV(long id, Guid userID, string userName)
    {
      await _requestservice.RevertKIV(id, userID, userName);
      return "OK";
    }

    [HttpPost]
    [Route("{id:int}/inspection")]
    public async Task<string> ScheduledInspection(long id, long jobID, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, Guid userID, string userName, string notes = null)
    {
      await _requestservice.ScheduledInspection(id, jobID, scheduledOn, scheduledOnTo,
        notes, userID, userName);
      return "Ok";
    }

    [HttpPost]
    [Route("recommend")]
    public async Task<string> Recommend([FromBody] RecommendParam recommendParam, Guid userID,
      string userName)
    {
      foreach (var review in recommendParam.reviews)
      {
        review.ReviewerID = userID;
        review.ReviewerName = userName;
      }

      await _requestservice.Recommend(recommendParam);

      return "Ok";
    }

    [HttpPost]
    [Route("review")]
    public async Task<string> Review([FromBody] RecommendParam recommendParam, Guid userID,
      string userName)
    {
      foreach (var review in recommendParam.reviews)
      {
        review.ReviewerID = userID;
        review.ReviewerName = userName;
      }

      await _requestservice.Review(recommendParam);

      return "Ok";
    }

    [HttpPost]
    [Route("approve")]
    public async Task<string> Approve([FromBody] IList<Review> reviews, Guid userID,
      string userName)
    {
      foreach (var review in reviews)
      {
        review.ReviewerID = userID;
        review.ReviewerName = userName;
      }

      await _requestservice.Approve(reviews);

      return "Ok";
    }

    [HttpPost]
    [Route("{id:int}/reaudit")]
    public async Task<string> Reaudit(long id, Guid userID, string userName,
      [FromBody] ReauditParam param)
    {
      await _requestservice.Reaudit(id, param.Notes, userID, userName); return "Ok";
    }

    [HttpGet]
    [Route("reviews")]
    public async Task<IList<Review>> GetReviews([FromQuery] long[] requestIDs = null)
    {
      return await _requestservice.GetReviews(requestIDs);
    }

    [HttpPost]
    [Route("{id:int}/reassign")]
    public async Task<string> Reassign(long id, Guid officerID, string officerName, string notes,
      Guid userID, string userName)
    {
      var toOfficer = new Officer
      {
        ID = officerID,
        Name = officerName
      };

      var user = new Officer
      {
        ID = userID,
        Name = userName
      };

      await _requestservice.Reassign(id, toOfficer, notes, user);

      return "Ok";
    }

    [HttpPut]
    [Route("{id:int}/status")]
    public async Task<string> UpdateStatus(long id, RequestStatus status, Guid userID,
      string userName, RequestStatusMinor? statusMinor = null)
    {
      await _requestservice.UpdateRequestStatus(id, status, statusMinor,
        new Officer(userID, userName));
      return "Okay";
    }

    [HttpPost]
    [Route("validateRequest")]
    public async Task<Model.Request> ValidateRequest(Scheme? scheme, SubScheme? subScheme,
      [FromBody] Premise premise)
    {
      return await _requestservice.ValidateRequest(scheme, subScheme, premise);
    }

    [HttpPut]
    [Route("{id:int}/proceedForReview")]
    public async Task<string> ProceedForReview(long id, Guid officerID, string officerName)
    {
      var assignofficer = new Officer
      {
        ID = officerID,
        Name = officerName
      };

      await _requestservice.ProceedForReview(id, assignofficer);
      return "Okay";
    }


    [HttpPut]
    [Route("{id:int}/reinstate")]
    public async Task<string> ReinsateRequest(long id, Guid userID, string userName,
      string notes, Guid? refID, string remarks)
    {
      var off = new Officer
      {
        ID = userID,
        Name = userName
      };

      var reason = new Master
      {
        ID = (Guid)refID,
        Value = remarks
      };

      await _requestservice.ReinstateRequest(id, notes, off, reason);
      return "Okay";
    }
  }
}
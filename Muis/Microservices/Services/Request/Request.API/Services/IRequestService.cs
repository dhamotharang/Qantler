using Core.Model;
using Request.API.DTO;
using Request.API.Params;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services
{
  public interface IRequestService
  {
    /// <summary>
    /// Gets all requets for a particular user.
    /// </summary>
    /// <returns>The request data.</returns>
    /// <param name="options">Request Options.</param>
    Task<IEnumerable<Model.Request>> QueryRequest(
      RequestOptions options);

    /// <summary>
    /// Get basic details of request
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.Request> GetRequestByIDBasic(long id);

    /// <summary>
    /// Get Related requests by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<Model.Request>> GetRelatedRequest(long id);

    /// <summary>
    /// Get Requests for an ID.
    /// </summary>
    /// <returns>The requets data.</returns>
    /// <param name="Id">Request identifier.</param>
    Task<Model.Request> GetRequestByID(long id);

    /// <summary>
    /// Get Requests for an ID.
    /// </summary>
    /// <returns>The requets data.</returns>
    /// <param name="refId">Request Id</param>
    /// <returns></returns>
    Task<Model.Request> GetRequestByRefID(string refId);

    /// <summary>
    /// Submit new Request.
    /// </summary>
    /// <returns>The Request created </returns>
    /// <param name="request"> Request.</param>
    Task<Model.Request> SubmitRequest(Model.Request request);

    /// <summary>
    /// Update the submitted request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="RFAID"></param>
    /// <returns></returns>
    Task<Model.Request> UpdateRequest(Model.Request request);

    /// <summary>
    /// Update ramarks and Status.
    /// </summary>
    /// <param name="requestID">Request identifier.</param>
    /// <param name="status">the escalate status</param>
    /// <param name="remarks> remarks</param>
    /// <param name="userid"> userid</param>
    /// <param name="username"> usernme</param>
    Task EscalateRequest(long requestID, EscalateStatus status, string remarks, Guid userid,
      string username);

    /// <summary>
    /// KIV request.
    /// </summary>
    /// <param name="id">the reference ID</param>
    /// <param name="notes">some notes</param>
    /// <param name="remindOn">the KIV reminder date</param>
    /// <param name="userID">user id</param>
    /// <param name="userName">user name</param>
    Task KIV(long id, string notes, DateTimeOffset remindOn, Guid userID, string userName);

    /// <summary>
    /// Revert request status from KIV.
    /// </summary>
    /// <param name="id">the request</param>
    /// <param name="userID">the user id</param>
    /// <param name="userName">user name</param>
    Task RevertKIV(long id, Guid userID, string userName);

    /// <summary>
    /// When inspection have been scheduled for the specified request.
    /// </summary>
    /// <param name="id">the request ID</param>
    /// <param name="jobID">the request job id</param>
    /// <param name="scheduledOn">the scheduled date</param>
    /// <param name="notes">some notes</param>
    /// <param name="userID">the user id</param>
    /// <param name="userName">the user name</param>
    /// <returns></returns>
    Task ScheduledInspection(long id, long jobID, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes, Guid userID, string userName);

    /// <summary>
    /// Submit review to request instance.
    /// </summary>
    /// <param name="recommendParam">the recommend param</param>
    Task Recommend(RecommendParam recommendParam);

    /// <summary>
    /// Submit review the CA rejection to request instance.
    /// </summary>
    /// <param name="recommendParam">the recommend param</param>
    Task Review(RecommendParam recommendParam);

    /// <summary>
    /// To approve request.
    /// </summary>
    /// <param name="review">the review</param>
    Task Approve(IList<Review> reviews);

    /// <summary>
    /// Revert request to auditor.
    /// </summary>
    /// <param name="id">the request ID</param>
    /// <param name="notes">some notes</param>
    /// <param name="userID">the user</param>
    /// <param name="userName">the user name</param>
    Task Reaudit(long id, string notes, Guid userID, string userName);

    /// <summary>
    /// Retrieve reviews for specified request.
    /// </summary>
    /// <param name="requestIDs">request IDs</param>
    /// <returns>the list of reviews</returns>
    Task<IList<Review>> GetReviews(long[] requestIDs);

    /// <summary>
    /// Reassigns a request to specified officer.
    /// </summary>
    /// <param name="requestID">request ID</param>
    /// <param name="toOfficer">to officer to reassign to</param>
    /// <param name="notes">some notes</param>
    /// <param name="user">the user who reassign</param>
    Task Reassign(long requestID, Officer toOfficer, string notes, Officer user);

    /// <summary>
    /// Invoked when a bill has been generated for specific request.
    /// </summary>
    Task OnBillGeneratedForRequest(long requestID, long billID);

    /// <summary>
    /// Invoked when bill has been paid for the specific request.
    /// </summary>
    Task OnBillPaid(long requestID, long billID);

    /// <summary>
    /// Update request status.
    /// </summary>
    Task UpdateRequestStatus(long requestID, RequestStatus status, RequestStatusMinor? statusMinor,
      Officer user);

    /// <summary>
    /// Validation application request before submit
    /// </summary>
    /// <param name="Scheme"></param>
    /// <param name="SubScheme"></param>
    /// <param name="premise"></param>
    /// <returns></returns>
    Task<Model.Request> ValidateRequest(Scheme? Scheme, SubScheme? SubScheme, Premise premise);

    /// <summary>
    /// Proceed for review
    /// </summary>
    /// <param name="id"></param>
    /// <param name="officer"></param>
    /// <returns></returns>
    Task ProceedForReview(long id, Officer officer);

    /// <summary>
    /// Reinstate the request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="notes"></param>
    /// <param name="user"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    Task ReinstateRequest(long id, string notes, Officer user, Master reason);
  }
}

using Core.Model;
using Request.API.Params;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Repository
{
  public interface IRequestRepository
  {
    /// <summary>
    /// Get basic request details.
    /// </summary>
    /// <returns>The requets data.</returns>
    /// <param name="Id">Request identifier.</param>
    public Task<Model.Request> GetRequestByIDBasic(long id);

    /// <summary>
    /// Get request info for an ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Request identifier.</returns>
    Task<Model.Request> GetRequestInfoByID(long id);
    /// <summary>
    /// Get requests for an ID.
    /// </summary>
    /// <returns>The requets data.</returns>
    /// <param name="Id">Request identifier.</param>
    public Task<Model.Request> GetRequestByID(long id);

    /// <summary>
    /// Get Requests for an Ref ID.
    /// </summary>
    /// <param name="refID"></param>
    /// <returns></returns>
    public Task<Model.Request> GetRequestByRefID(string refID);

    /// <summary>
    /// Get Requests for a user.
    /// </summary>
    /// <returns>The requets data.</returns>
    /// <param name="options">Request Options.</param>
    public Task<IEnumerable<Model.Request>> Select(
      RequestOptions options);

    /// <summary>
    /// Get Related requests based on request Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IEnumerable<Model.Request>> SelectRelatedRequests(long id);

    /// <summary>
    /// Insert new  Request.
    /// </summary>
    /// <returns>The ID of newly inserted Request.</returns>
    /// <param name="req">Request model</param>
    public Task<long> InsertRequest(Model.Request req);

    /// <summary>
    /// Update request on RFA response with HC02 amendment
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<long> UpdateRequest(Model.Request req);

    /// <summary>
    /// Get Request ID from Characteristic.
    /// </summary>
    /// <returns>ID</returns>
    /// <param name="characteristic">Characteristic.</param>
    public Task<long> GetRequestIDFromCharacteristic(
      Characteristic characteristic);

    /// <summary>
    /// Get Message by locale and key.
    /// </summary>
    /// <returnsmessage</returns>
    /// <param name="locale">Locale</param>
    /// <param name="key">string</param>
    public Task<string> GetMessageByKeyAndLocale(int locale, string key);

    /// <summary>
    /// Request escalation actions.
    /// </summary>
    /// <param name="id">request ID</param>
    /// <param name="status">escalation status</param>
    /// <param name="remarks">remarks</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    public Task EscalateAction(long id, EscalateStatus status, string remarks, Guid userID,
      string userName);

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
    /// Update request status.
    /// </summary>
    /// <param name="id">reference id</param>
    /// <param name="status">to new status</param>
    /// <param name="statusMinor">to new status minor</param>
    Task UpdateStatus(long id, RequestStatus status, RequestStatusMinor? statusMinor,
      RequestStatus? oldStatus = null);

    /// <summary>
    /// Maps log to request.
    /// </summary>
    Task MapLog(long requestID, long logID);

    /// <summary>
    /// Insert review.
    /// </summary>
    /// <param name="review">the review to insert</param>
    /// <returns></returns>
    Task<long> InsertReview(Review review);

    /// <summary>
    /// Insert action history instance.
    /// </summary>
    /// <param name="entity">the entity to insert</param>
    /// <returns>the ID of the inserted entity</returns>
    Task<long> InsertActionHistory(RequestActionHistory entity);

    /// <summary>
    /// Retrieve request action history instances.
    /// </summary>
    /// <param name="requestID">the request ID</param>
    /// <returns>the request action history instances</returns>
    Task<IList<RequestActionHistory>> GetRequestActionHistories(long requestID);

    /// <summary>
    /// Assign an officer to the specified request.
    /// </summary>
    /// <param name="requestID">the request ID</param>
    /// <param name="officerID">the officer ID</param>
    /// <param name="officerName">the officerName</param>
    Task SetAssignedOfficer(long requestID, Guid? officerID, string officerName);

    /// <summary>
    /// Query review based on specified filter options.
    /// </summary>
    /// <param name="filter">the filter options</param>
    /// <returns>the review instances</returns>
    Task<IList<Review>> QueryReview(ReviewFilter filter);

    /// <summary>
    /// Maps request to job order ID.
    /// </summary>
    /// <param name="requestID">the request</param>
    /// <param name="jobID">the reference id of the job order</param>
    Task MapJobOrderToRequest(long requestID, long jobID);

    /// <summary>
    /// Update request line item
    /// </summary>
    /// <param name="requestLineItem"></param>
    /// <returns></returns>
    Task<long> UpdateRequestLineItem(RequestLineItem requestLineItem, long RequestID);

    /// <summary>
    /// Update request line item characteristic
    /// </summary>
    /// <param name="chars"></param>
    /// <param name="LineItemID"></param>
    /// <returns></returns>
    Task UpdateRequestLineItemCharacteristics(Characteristic chars, long LineItemID);

    /// <summary>
    /// Insert request attachements
    /// </summary>
    /// <param name="attachment"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task<long> InsertRequestAttachments(Attachment attachment, long RequestID);

    /// <summary>
    /// Update request premise
    /// </summary>
    /// <param name="premise"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task UpdateRequestPremise(Premise premise, long RequestID);

    /// <summary>
    /// Update request menu
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task UpdateRequestMenu(Menu menu, long RequestID);

    /// <summary>
    /// Update request ingredient
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task UpdateRequestIngredient(Ingredient ingredient, long RequestID);

    /// <summary>
    /// Update request characteristics
    /// </summary>
    /// <param name="chars"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task UpdateRequestCharacteristics(Characteristic chars, long RequestID);

    /// <summary>
    /// Update request halal team
    /// </summary>
    /// <param name="halalTeam"></param>
    /// <param name="RequestID"></param>
    /// <returns></returns>
    Task UpdateRequestHalalTeam(HalalTeam halalTeam, long RequestID);

    /// <summary>
    /// Validate the application requests before submit
    /// </summary>
    /// <param name="Scheme"></param>
    /// <param name="SubScheme"></param>
    /// <param name="premise"></param>
    /// <returns></returns>
    Task<Model.Request> ValidateRequest(Scheme? Scheme, SubScheme? SubScheme, Premise premise);

    /// <summary>
    /// Get Parent request
    /// </summary>
    /// <param name="Scheme"></param>
    /// <param name="SubScheme"></param>
    /// <param name="premise"></param>
    /// <returns></returns>
    Task<Model.Request> GetParentRequest
     (Scheme? Scheme, SubScheme? SubScheme, Premise premise,
      RequestStatus[] statuses, RequestType[] requestTypes);

  }

  public class ReviewFilter
  {
    public long[] RequestIDs { get; set; }
  }
}

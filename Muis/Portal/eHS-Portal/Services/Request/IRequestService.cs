using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.DTO;
using eHS.Portal.Model;

namespace eHS.Portal.Services
{
  public interface IRequestService
  {
    Task<IList<Request>> Search(RequestOptions options);

    /// <summary>
    /// Get Related requests by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Request>> GetRelatedRequest(long id);

    /// <summary>
    /// Retrieve request details with specified ID.
    /// </summary>
    Task<Request> GetByID(long id);

    /// <summary>
    /// Retrieve request basic details.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Request> GetByIDBasic(long id);

    /// <summary>
    /// Request escalation actions.
    /// </summary>
    Task DoEscalate(long requestID, EscalateStatus status, string remarks);

    /// <summary>
    /// KIV Request.
    /// </summary>
    Task KIV(long id, string notes, DateTimeOffset remindOn);

    /// <summary>
    /// Revert KIV to previous status.
    /// </summary>
    Task RevertKIV(long id);

    /// <summary>
    /// Schedule an inspection for the specified request.
    /// </summary>
    Task<JobOrder> ScheduleInspection(long id, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes, Officer officer);

    /// <summary>
    /// Bulk review list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    Task BulkReviewIngredients(IList<Ingredient> ingredients);

    /// <summary>
    /// Bulk review list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    Task BulkReviewMenus(IList<Menu> menus);

    /// <summary>
    /// Submit recommendation to request.
    /// </summary>
    /// <param name="review">the recommendation</param>
    Task Recommend(IList<Review> reviews);

    /// <summary>
    /// Submit review to request for CA recommend for rejection.
    /// </summary>
    /// <param name="review">the recommendation</param>
    Task Review(IList<Review> reviews);

    /// <summary>
    /// Approve or reject request
    /// </summary>
    /// <param name="reviews">the reviews</param>
    Task Approve(IList<Review> reviews);

    /// <summary>
    /// Submit request for re-audit.
    /// </summary>
    /// <param name="requestID">the request ID</param>
    /// <param name="remarks">some remakrs</param>
    Task Reaudit(long requestID, string remarks);

    /// <summary>
    /// Retrieve reviews for the specified request.
    /// </summary>
    Task<IList<Review>> GetReviews(long[] requestIDs);

    /// <summary>
    /// Re-assigns a request to specified officer.
    /// </summary>
    /// <param name="requestID">the request ID</param>
    /// <param name="toOfficer">the officer</param>
    /// <param name="notes">some notes</param>
    Task Reassign(long requestID, Officer toOfficer, string notes);

    /// <summary>
    /// Updates request status.
    /// </summary>
    Task ProceedToPayment(long id, long billID, IList<BillLineItem> billLines);

    /// <summary>
    /// Updates request status.
    /// </summary>
    Task UpdateRequestStatus(long id, RequestStatus status, RequestStatusMinor? statusMinor);

    /// <summary>
    /// Get notes for specified request.
    /// </summary>
    Task<IList<Notes>> GetNotes(long id);

    /// <summary>
    /// Add notes to specified request.
    /// </summary>
    Task<Notes> AddNotes(long id, Notes notes);

    /// <summary>
    /// Proceed for review the request
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task ProceedForReview(long id);

    /// <summary>
    /// Reinstate the request
    /// </summary>
    /// <param name="requestID"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task Reinstate(long requestID, ReinstateParam param);
  }
}

using Core.Model;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IJobOrderRepository
  {
    public Task<IEnumerable<Model.JobOrder>> Select(JobOrderOptions options);

    /// <summary>
    /// Insert job order entity.
    /// </summary>
    /// <param name="model">the entity to insert</param>
    /// <param name="userID">user ID</param>
    /// <param name="userName">user name</param>
    /// <returns>the ID of the newly inserted entity</returns>
    Task<long> InsertJobOrder(Model.JobOrder model);

    /// <summary>
    /// To update job order.
    /// </summary>
    Task UpdateJobOrder(Model.JobOrder model);

    /// <summary>
    /// Gets all updated joborders for a particular auditor.
    /// </summary>
    /// <returns>Job order list.</returns>
    /// <param name="assignedTo">Auditor ID.</param>
    /// <param name="lastupdatedOn">last updated on</param>
    Task<IEnumerable<Model.JobOrder>> GetJobOrders(
      Guid assignedTo,
      DateTimeOffset? lastupdatedOn);

    /// <summary>
    /// Update Job Order State.
    /// </summary>
    /// <returns>Updated Job Order  </returns>
    /// <param name="Id">Job Order identifier.</param>
    /// <param name="newStatus"> JobOrderStatus</param>
    Task UpdateJobOrderStatus(long id, JobOrderStatus newStatus);

    /// <summary>
    /// Get basic job order details.
    /// </summary>
    /// <returns>The job order data.</returns>
    /// <param name="Id">Job Order identifier.</param>
    Task<Model.JobOrder> GetJobOrderByIDBasic(long id);

    /// <summary>
    /// Get joborder by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.JobOrder> GetJobOrderByID(long id);

    /// <summary>
    /// To add invitee to job order with specified ID.
    /// </summary>
    Task AddInvitee(long id, Officer officer);

    /// <summary>
    /// To remove invitee to job order with specified ID.
    /// </summary>
    Task RemoveInvitee(long id, Guid officerID);

    /// <summary>
    /// Add List of attendees for a joborder
    /// </summary>
    /// <param name="id"></param>
    /// <param name="attendees"></param>
    /// <returns></returns>
    Task AddAttendees(long id, IList<Attendee> attendees);

    /// <summary>
    /// Get List of attendees for a joborder
    /// </summary>
    /// <param name="id"></param>
    /// <returns> List of attendees</returns>
    Task<IEnumerable<Attendee>> GetAttendeesByJobID(long ID);

    /// <summary>
    /// Update attendees for a job order.
    /// </summary>
    /// <returns>Success</returns>
    /// <param name="Id">Job Order identifier.</param>
    /// <param name="attendees> List of attendees</param>
    Task UpdateAttendees(long id, IList<Attendee> attendees);

    /// <summary>
    /// Maps log to job order.
    /// </summary>
    Task MapLog(long ID, long logID);

    /// <summary>
    /// Insert JobOrder Reschedule History.
    /// </summary>
    Task<long> InsertRescheduleHistory(long jobID, Guid masterID, string notes);

    /// <summary>
    /// Insert job order emails
    /// </summary>
    Task MapEmail(long jobID, long emailID);

    /// <summary>
    /// Get joborderemail by job id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<long>> GetEmails(long id);
  }
}
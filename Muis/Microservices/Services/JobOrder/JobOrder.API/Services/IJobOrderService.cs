using Core.Model;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public interface IJobOrderService
  {
    /// <summary>
    /// Query job order based on specified criterias.
    /// </summary>
    Task<IEnumerable<Model.JobOrder>> QueryJobOrder(JobOrderOptions options);

    /// <summary>
    /// Create job order
    /// </summary>
    /// <param name="model">the job order instance to create</param>
    /// <param name="userID">user ID</param>
    /// <param name="userName">user name</param>
    /// <returns>the newly created job order</returns>
    Task<Model.JobOrder> Create(ScheduleJobOrderParam param, Guid userID, string userName, 
      string userEmail);

    /// <summary>
    /// Gets updated joborders for a particular auditor.
    /// </summary>
    /// <returns>Job order list.</returns>
    /// <param name="assignedTo">Auditor ID.</param>
    /// <param name="lastupdatedOn">last updated on</param>
    Task<IEnumerable<Model.JobOrder>> GetJobOrders(Guid assignedTo, DateTimeOffset? lastupdatedOn);

    /// <summary>
    /// Update Job Order State.
    /// </summary>
    /// <returns>Updated job order model  </returns>
    /// <param name="Id">Job Order identifier.</param>
    /// <param name="newStatus"> JobOrderStatus</param>
    Task<Model.JobOrder> UpdateJobOrderStatus(long id, JobOrderStatus newStatus);

    /// <summary>
    /// Get joborder by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Model.JobOrder> GetJobOrderByID(long id);

    /// <summary>
    /// insert into invitee by joborder id
    /// </summary>
    Task AddInvitee(long jobID, Officer officer, Officer user);

    /// <summary>
    /// Delete invitee by inviteeid and jobid
    /// </summary>
    Task DeleteInvitee(long jobID, Guid officerID, Officer user);

    /// <summary>
    /// Add List of attendees for a joborder
    /// </summary>
    /// <param name="id"></param>
    /// <param name="attendees"></param>
    /// <returns>List of attendees</returns>
    Task<IEnumerable<Attendee>> AddAttendees(long id, IList<Attendee> attendees);

    /// <summary>
    /// Update attendee for a job order.
    /// </summary>
    /// <returns>Success</returns>
    /// <param name="Id">Job identifier.</param>
    /// <param name="> List of attendees</param>
    Task UpdateAttendees(long id, IList<Attendee> attendees);

    /// <summary>
    /// Scheduled for a job order.
    /// </summary>
    Task Reschedule(long id, RescheduleParam param, Officer user);

    /// <summary>
    /// Cancel  a job order.
    /// </summary>
    Task Cancel(long id, CancelParam param, Officer user);

    /// <summary>
    /// Schedule periodic inspection
    /// </summary>
    Task Schedule(long id, ScheduleParam param, Officer user);
  }
}
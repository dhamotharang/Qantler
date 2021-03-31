using eHS.Portal.Client;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Models.JobOrder;
using System;
using eHS.Portal.DTO;

namespace eHS.Portal.Services
{
  public interface IJobOrderService
  {
    /// <summary>
    /// Get all job order details on load with advance filter
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IList<JobOrder>> Search(JobOrderOptions options);

    /// <summary>
    /// Get job order details by joborder id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<JobOrder> GetByID(long id);

    /// <summary>
    /// Adding invitees
    /// </summary>
    /// <param name="id"></param>
    /// <param name="jobid"></param>
    /// <returns></returns>
    Task AddInvitee(long id, Officer officer);

    /// <summary>
    /// Delete invitees of joborder
    /// </summary>
    Task DeleteInvitee(long id, Guid officerID);

    /// <summary>
    /// Rescheduled inspection of joborder
    /// </summary>
    Task Reschedule(long id, RescheduleParam param);

    /// <summary>
    /// Cancel a joborder
    /// </summary>
    Task Cancel(long id, CancelParam param);

    /// <summary>
    /// Get notes for specified joborder.
    /// </summary>
    Task<IList<Notes>> GetNotes(long id);

    /// <summary>
    /// Add notes to specified joborder.
    /// </summary>
    Task<Notes> AddNotes(long id, Notes notes);

    /// <summary>
    /// Schedule periodic inspection
    /// </summary>
    Task Schedule(long id, ScheduleParam param);
  }
}
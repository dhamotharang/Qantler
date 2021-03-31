using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobOrder.Model;
using JobOrder.API.Services;
using JobOrder.API.Models;
using System.Linq;
using Core.Model;

namespace API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class JobOrderController : ControllerBase
  {
    readonly IJobOrderService _jobOrderService;

    public JobOrderController(IJobOrderService jobOrderService)
    {
      _jobOrderService = jobOrderService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IEnumerable<JobOrder.Model.JobOrder>> Query(
      long? id = null,
      string customer = null,
      string premise = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      [FromQuery] Guid[] assignedTo = null,
      [FromQuery] JobOrderStatus[] status = null,
      [FromQuery] JobOrderType[] type = null)
    {
      var result = await _jobOrderService.QueryJobOrder(new JobOrderOptions
      {
        ID = id,
        Customer = customer,
        Premise = premise,
        AssignedTo = assignedTo,
        From = from,
        To = to,
        Status = status?.ToList(),
        Type = type?.ToList()
      });
      return result;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<JobOrder.Model.JobOrder> Get(long id)
    {
      return await _jobOrderService.GetJobOrderByID(id);
    }

    [HttpPost]
    [Route("{id}/invitee")]
    public async Task<string> Add(long id, [FromBody] Officer officer, Guid userID, string userName)
    {
      await _jobOrderService.AddInvitee(id, officer, new Officer(userID, userName));
      return "Ok";
    }

    [HttpDelete]
    [Route("{id}/invitee/{officerID}")]
    public async Task<string> Delete(long id, Guid officerID, Guid userID, string userName)
    {
      await _jobOrderService.DeleteInvitee(id, officerID, new Officer(userID, userName));
      return "Ok";
    }

    [HttpPost]
    public async Task<JobOrder.Model.JobOrder> Submit([FromBody] ScheduleJobOrderParam param,
      Guid userID, string userName, string userEmail)
    {
      return await _jobOrderService.Create(param, userID, userName, userEmail);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<JobOrder.Model.JobOrder>> Get(
      Guid assignedTo, DateTimeOffset? lastupdatedOn = null)
    {
      return await _jobOrderService.GetJobOrders(assignedTo, lastupdatedOn);
    }

    [HttpPost]
    [Route("{id}/status")]
    public async Task<JobOrder.Model.JobOrder> Post(long id, JobOrderStatus status)
    {
      return await _jobOrderService.UpdateJobOrderStatus(id, status);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IEnumerable<Attendee>> Post(
      long id,
      [FromBody] IList<Attendee> attendees)
    {
      return await _jobOrderService.AddAttendees(id, attendees);
    }

    [HttpPut]
    [Route("{id}/attendee")]
    public async Task<string> Put(
      long id,
      [FromBody] IList<Attendee> attendees)
    {
      await _jobOrderService.UpdateAttendees(id, attendees);

      return "success";
    }

    [HttpPut]
    [Route("{id:int}/reschedule")]
    public async Task<string> Reschedule(long id, [FromBody] RescheduleParam param, Guid userID,
      string userName, string userEmail)
    {
      await _jobOrderService.Reschedule(id, param, new Officer(userID, userName, userEmail));
      return "Ok";
    }

    [HttpPost]
    [Route("{id:int}/cancel")]
    public async Task<string> Cancel(long id, [FromBody] CancelParam param, Guid userID,
      string userName)
    {
      await _jobOrderService.Cancel(id, param, new Officer(userID, userName));
      return "Ok";
    }

    [HttpPut]
    [Route("{id:int}/schedule")]
    public async Task<string> Schedule(long id, [FromBody] ScheduleParam param, Guid userID,
      string userName)
    {
      await _jobOrderService.Schedule(id, param, new Officer(userID, userName));
      return "Ok";
    }
  }
}
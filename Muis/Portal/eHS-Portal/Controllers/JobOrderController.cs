using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Model;
using eHS.Portal.Models.JobOrder;
using Microsoft.AspNetCore.Mvc;
using eHS.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using eHS.Portal.Client;
using System;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Extensions;
using Core.Http.Exceptions;
using eHS.Portal.DTO;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.RequestRead,
    Permission.RequestReview,
    Permission.RequestApprove,
    Permission.RequestInvoice,
    Permission.RequestIssuance,
    Permission.RequestReassign,
    Permission.RequestEscalateAction,
    Permission.RequestOverride,
    Permission.RequestReview,
    Permission.PeriodicInspectionRead,
    Permission.PeriodicInspectionReadWrite)]
  public class JobOrderController : Controller
  {
    readonly IJobOrderService _joborderService;
    readonly IIdentityService _identityService;

    public JobOrderController(IJobOrderService joborderService, IIdentityService identityService)
    {
      _joborderService = joborderService;
      _identityService = identityService;
    }

    public async Task<IActionResult> Index()
    {
      Guid? assignedTo = null;
      JobOrderType? defaultType = null;

      if (User.HasPermission(Permission.RequestReview))
      {
        defaultType = JobOrderType.Audit;

        assignedTo = User.GetUserId();
      }
      else if (User.HasPermission(Permission.PeriodicInspectionReadWrite))
      {
        defaultType = JobOrderType.Periodic;

        assignedTo = User.GetUserId();
      }

      var model = new IndexModel
      {
        AssignedTo = assignedTo,
        DefaultType = defaultType,
        DefaultStatuses = new List<JobOrderStatus>
        {
          JobOrderStatus.Pending
        },
        Users = await _identityService.List(new IdentityFilter
        {
          Permissions = new Permission[]
          {
            Permission.RequestRead,
            Permission.RequestReview,
            Permission.RequestApprove,
            Permission.RequestInvoice,
            Permission.RequestIssuance,
            Permission.RequestReassign,
            Permission.RequestEscalateAction,
            Permission.RequestOverride
          }
        })
      };
      return View(model);
    }

    [Route("[controller]/details/{id}")]
    [ServiceFilter(typeof(SessionAwareFilter))]
    public async Task<IActionResult> Details(long id)
    {
      return View(new DetailsModel
      {
        ID = id,
        Data = await _joborderService.GetByID(id)
      });
    }

    [Route("api/[controller]/index")]
    public async Task<IList<JobOrder>> IndexData(long? id = null,
      string customer = null,
      string premise = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      [FromQuery] IList<JobOrderStatus> status = null,
      [FromQuery] IList<JobOrderType> type = null,
      [FromQuery] Guid[] assingedTo = null)
    {
      var result = await _joborderService.Search(new JobOrderOptions
      {
        ID = id,
        Customer = customer,
        Premise = premise,
        Status = status,
        Type = type,
        AssignedTo = assingedTo,
        From = from,
        To = to
      });
      return result;
    }

    [HttpPost]
    [Route("api/[controller]/{id}/invitee")]
    public async Task<string> Invite(long id, [FromBody] Officer officer)
    {
      await _joborderService.AddInvitee(id, officer);
      return "Ok";
    }

    [HttpDelete]
    [Route("api/[controller]/{id}/invitee/{officerID}")]
    public async Task<string> DeleteInvitee(long id, Guid officerID)
    {
      await _joborderService.DeleteInvitee(id, officerID);
      return "Ok";
    }

    [HttpGet]
    [Route("api/[controller]/{id}")]
    public async Task<JobOrder> GetByID(long id)
    {
      var result = await _joborderService.GetByID(id);
      if (result == null)
      {
        throw new NotFoundException();
      }
      return result;
    }

    [HttpPut]
    [Route("api/[controller]/{id:int}/reschedule")]
    public async Task<string> Reschedule(long id, [FromBody] RescheduleParam param)
    {
      await _joborderService.Reschedule(id, param);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/cancel")]
    public async Task<string> Cancel(long id, [FromBody] CancelParam param)
    {
      await _joborderService.Cancel(id, param);
      return "Ok";
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/notes")]
    public async Task<IList<Notes>> GetNotes(long id)
    {
      return await _joborderService.GetNotes(id);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/notes")]
    public async Task<Notes> AddNotes(long id, [FromBody] Notes notes)
    {
      return await _joborderService.AddNotes(id, notes);
    }

    [HttpPut]
    [Route("api/[controller]/{id:int}/schedule")]
    public async Task<string> Schedule(long id, [FromBody] ScheduleParam param)
    {
      await _joborderService.Schedule(id, param);
      return "Ok";
    }
  }
}
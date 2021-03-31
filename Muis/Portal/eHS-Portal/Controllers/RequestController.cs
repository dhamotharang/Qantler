using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.DTO;
using eHS.Portal.Extensions;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Request;
using eHS.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.RequestRead,
    Permission.RequestReview,
    Permission.RequestReviewApproval,
    Permission.RequestApprove,
    Permission.RequestInvoice,
    Permission.RequestIssuance,
    Permission.RequestReassign,
    Permission.RequestEscalateAction,
    Permission.RequestOverride)]
  public class RequestController : Controller
  {
    readonly IRequestService _requestService;
    readonly IIdentityService _identityService;

    public RequestController(IRequestService requestService, IIdentityService identityService)
    {
      _requestService = requestService;
      _identityService = identityService;
    }

    public async Task<IActionResult> Index(Guid? customerID = null)
    {
      Guid? assignedTo = null;

      Guid? defaultCustomerID = null;
      IList<RequestStatus> defaultStatus = null;

      if (customerID != null)
      {
        defaultCustomerID = customerID;
      }
      else if (User.HasPermission(Permission.RequestReview))
      {
        defaultStatus = new List<RequestStatus>
        {
          RequestStatus.Open,
          RequestStatus.ForInspection
        };

        assignedTo = User.GetUserId();
      }
      else
      {
        defaultStatus = new List<RequestStatus>();

        if (User.HasPermission(Permission.RequestReassign)
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.Open);
        }

        if (User.HasPermission(Permission.RequestReviewApproval)
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.PendingReviewApproval);
        }

        if (User.HasPermission(Permission.RequestApprove)
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.PendingApproval);
        }

        if (User.HasPermission(Permission.RequestInvoice)
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.PendingBill);
        }

        if ((User.HasPermission(Permission.PaymentRead)
          || User.HasPermission(Permission.PaymentReadWrite))
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.PendingPayment);
        }

        if (User.HasPermission(Permission.RequestIssuance)
          && !User.HasPermission(Permission.RequestOverride))
        {
          defaultStatus.Add(RequestStatus.Issuance);
        }
      }

      if (defaultStatus?.Count == 0)
      {
        defaultStatus.Add(RequestStatus.Open);
      }

      return View(new IndexModel
      {
        CustomerId = defaultCustomerID,
        AssignedTo = assignedTo,
        DefaultStatuses = defaultStatus,
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
      });
    }

    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
      return View(new DetailsModel
      {
        ID = id,
        CurrentUserID = User.GetUserId(),
        Data = await _requestService.GetByID(id)
      });
    }

    [Route("api/[controller]/index")]
    public async Task<IList<Request>> IndexData(long id = 0,
      string refID = null,
      string customerCode = null,
      Guid? customerID = null,
      string customer = null,
      string premise = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null,
      IList<RequestStatus> status = null,
      IList<RequestType> type = null,
      Guid[] assingedTo = null,
      RFAStatus? rfaStatus = null,
      EscalateStatus? escalateStatus = null,
      IList<RequestStatusMinor> statusMinor = null)
    {
      var options = new RequestOptions
      {
        ID = id,
        CustomerID = customerID,
        RefID = refID,
        Customer = customer,
        CustomerCode = customerCode,
        Premise = premise,
        Status = status,
        Type = type,
        From = from,
        To = to,
        AssignedTo = assingedTo,
        RFAStatus = rfaStatus,
        EscalateStatus = escalateStatus,
        StatusMinor = statusMinor
      };

      return await _requestService.Search(options);
    }

    [HttpGet]
    [Route("api/[controller]/{id}/related")]
    public async Task<IList<Request>> GetRelatedRequest(long id)
    {
      return await _requestService.GetRelatedRequest(id);
    }

    [HttpGet]
    [Route("api/[controller]/{id}/basic")]
    public async Task<Request> GetRequestByIDBasic(long id)
    {
      return await _requestService.GetByIDBasic(id);
    }

    [HttpPost]
    [Route("api/[controller]/{id}/escalate")]
    public async Task<string> Escalate(long id, EscalateStatus status, string remarks)
    {
      await _requestService.DoEscalate(id, status, remarks);

      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/{id}/kiv")]
    public async Task<string> KIV(long id, string notes, DateTimeOffset remindOn)
    {
      await _requestService.KIV(id, notes, remindOn);
      return "OK";
    }

    [HttpDelete]
    [Route("api/[controller]/{id}/kiv")]
    public async Task<string> RevertKIV(long id)
    {
      await _requestService.RevertKIV(id);
      return "OK";
    }

    [HttpPost]
    [Route("api/[controller]/{id}/schedule")]
    public async Task<JobOrder> ScheduleInspection(long id, DateTimeOffset scheduledOn,
      DateTimeOffset scheduledOnTo, string notes = null)
    {
      return await _requestService.ScheduleInspection(id, scheduledOn, scheduledOnTo, notes,
        new Officer(User.GetUserId(), User.GetName()));
    }

    [HttpPut]
    [Route("api/[controller]/ingredients/review")]
    public async Task<string> SaveIngredients([FromBody] IList<Ingredient> ingredients)
    {
      await _requestService.BulkReviewIngredients(ingredients);
      return "Ok";
    }

    [HttpPut]
    [Route("api/[controller]/menus/review")]
    public async Task<string> SaveMenus([FromBody] IList<Menu> menus)
    {
      await _requestService.BulkReviewMenus(menus);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/recommend")]
    public async Task<string> Recommend([FromBody] IList<Review> reviews)
    {
      await _requestService.Recommend(reviews);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/review")]
    public async Task<string> Review([FromBody] IList<Review> reviews)
    {
      await _requestService.Review(reviews);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/approve")]
    [PermissionFilter(Permission.RequestApprove,
      Permission.RequestOverride)]
    public async Task<string> Approve([FromBody] IList<Review> reviews)
    {
      await _requestService.Approve(reviews);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/reaudit")]
    [PermissionFilter(Permission.RequestApprove,
      Permission.RequestOverride)]
    public async Task<string> Reaudit(long id, string remarks)
    {
      await _requestService.Reaudit(id, remarks);

      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/reassign")]
    [PermissionFilter(Permission.RequestReassign,
      Permission.RequestOverride)]
    public async Task<string> Reassign(long id, Guid officerID, string officerName, string notes)
    {
      await _requestService.Reassign(id,
        new Officer
        {
          ID = officerID,
          Name = officerName
        },
        notes);

      return "Ok";
    }

    [HttpGet]
    [Route("api/[controller]/reviews")]
    public async Task<IList<Review>> GetReviews([FromQuery] long[] requestIDs)
    {
      return await _requestService.GetReviews(requestIDs);
    }

    [HttpPut]
    [Route("api/[controller]/{id:int}/status")]
    public async Task<string> UpdateRequestStatus(long id, RequestStatus status,
      RequestStatusMinor? statusMinor = null)
    {
      await _requestService.UpdateRequestStatus(id, status, statusMinor);
      return "Ok";
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/status/payment")]
    [PermissionFilter(Permission.RequestInvoice,
      Permission.RequestOverride)]
    public async Task<string> UpdateRequestStatus(long id, long billID,
      [FromBody] IList<BillLineItem> lineItems)
    {
      await _requestService.ProceedToPayment(id, billID, lineItems);
      return "Ok";
    }

    [HttpGet]
    [Route("api/[controller]/{id:int}/notes")]
    public async Task<IList<Notes>> GetNotes(long id)
    {
      return await _requestService.GetNotes(id);
    }

    [HttpPost]
    [Route("api/[controller]/{id:int}/notes")]
    public async Task<Notes> AddNotes(long id, [FromBody] Notes notes)
    {
      return await _requestService.AddNotes(id, notes);
    }

    [HttpPost]
    [Route("api/[controller]/proceedreview")]
    [PermissionFilter(Permission.SetupCustomerCode)]
    public async Task<string> ProceedForReview(long id)
    {
      await _requestService.ProceedForReview(id);
      return "Ok";
    }

    [HttpPut]
    [Route("api/[controller]/{id:int}/reinstate")]
    [PermissionFilter(Permission.RequestOverride)]
    public async Task<string> Reinstate(long id, [FromBody] ReinstateParam param)
    {
      await _requestService.Reinstate(id, param);
      return "Ok";
    }
  }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Extensions;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.RFA;
using eHS.Portal.Services;
using eHS.Portal.Services.RFA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.RequestRead,
    Permission.RequestReview,
    Permission.RequestApprove,
    Permission.RFASupport,
    Permission.RequestReviewApproval)]
  [ServiceFilter(typeof(SessionAwareFilter))]
  public class RFAController : Controller
  {
    readonly IRFAService _rfaService;
    readonly IIdentityService _identityService;

    public RFAController(IRFAService rfaService, IIdentityService identityService)
    {
      _rfaService = rfaService;
      _identityService = identityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      Guid? raisedBy = User.HasPermission(Permission.RequestReview, Permission.RequestOverride)
                    ? User.GetUserId()
                    : default(Guid?);

      var data = await _rfaService.ListOfRFA(new RFAOptions
      {
        RaisedBy = raisedBy,
        Status = new List<RFAStatus> { RFAStatus.Open, RFAStatus.PendingReview }
      });

      return View(new IndexModel
      {
        Data = data,
        Users = await _identityService.List(new IdentityFilter
        {
          Permissions = new Permission[]
          {
            Permission.RequestReview,
            Permission.RequestOverride,
          }
        })
      });
    }

    [HttpGet]
    [Route("[controller]/details/{id:int}")]
    public async Task<IActionResult> Details(long id)
    {
      return View(await _rfaService.GetByID(id));
    }

    [HttpGet]
    [Route("api/[controller]/list")]
    public async Task<IList<RFA>> List(long? id = null,
      Guid? raisedBy = null,
      string customer = null,
      DateTimeOffset? createdOn = null,
      DateTimeOffset? dueOn = null,
      [FromQuery] IList<RFAStatus> status = null)
    {
      return await _rfaService.ListOfRFA(new RFAOptions
      {
        ID = id,
        Customer = customer,
        Status = status,
        RaisedBy = raisedBy,
        CreatedOn = createdOn,
        DueOn = dueOn
      });
    }

    [HttpPost]
    [Route("api/[controller]")]
    public async Task<RFA> Submit([FromBody] RFA data)
    {
      data.RaisedBy = User.GetUserId();
      data.RaisedByName = User.GetName();

      return await _rfaService.Submit(data);
    }

    [HttpGet]
    [Route("api/[controller]/{id}")]
    public async Task<RFA> GetByID(long id)
    {
      return await _rfaService.GetByID(id);
    }

    [HttpDelete]
    [Route("api/[controller]/{id}")]
    public async Task<string> Discard(long id)
    {
      await _rfaService.Discard(id);
      return "success";
    }

    [HttpPost]
    [Route("api/[controller]/{id}/close")]
    public async Task<string> Close(long id)
    {
      await _rfaService.Close(id);
      return "success";
    }

    [HttpPost]
    [Route("api/[controller]/{id}/extend")]
    public async Task<string> Extend(long id, string notes, DateTimeOffset toDate)
    {
      await _rfaService.Extend(id, notes, toDate);
      return "success";
    }
  }
}

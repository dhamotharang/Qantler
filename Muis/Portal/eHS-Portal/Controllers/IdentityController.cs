using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  public class IdentityController : Controller
  {
    readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
      _identityService = identityService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    [Route("api/identity/{id}")]
    public async Task<Identity> GetByID(Guid id)
    {
      return await _identityService.GetIdentityByID(id);
    }

    [HttpGet]
    [Route("api/identity/list")]
    public async Task<IList<Identity>> List(string name = null,
      string email = null,
      [FromQuery] RequestType[] requestTypes = null,
      [FromQuery] Permission[] permissions = null,
      [FromQuery] long[] clusters = null)
    {
      return await _identityService.List(new IdentityFilter
      {
        Name = name,
        Email = email,
        Clusters = clusters,
        RequestTypes = requestTypes,
        Permissions = permissions
      });
    }
  }
}
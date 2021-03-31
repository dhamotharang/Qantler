using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Http.Exceptions;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.User;
using eHS.Portal.Services;
using eHS.Portal.Services.Cluster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.UserRead,
      Permission.UserReadWrite)]
  public class UserController : Controller
  {
    readonly IIdentityService _identityService;
    readonly IClusterService _clusterService;

    public UserController(IIdentityService identityService, IClusterService clusterService)
    {
      _identityService = identityService;
      _clusterService = clusterService;
    }

    public async Task<IActionResult> Index()
    {
      var clusters = await _clusterService.Search();

      return View(new IndexModel
      {
        Clusters = clusters
      });
    }

    [Route("[controller]/details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
      var clusters = await _clusterService.Search();

      var data = await _identityService.GetIdentityByID(id);

      return View(new DetailsModel
      {
        ID = id,
        Data = data,
        Clusters = clusters
      });
    }

    [Route("[controller]/form")]
    [Route("[controller]/form/{id}")]
    [PermissionFilter(Permission.UserReadWrite)]
    public async Task<IActionResult> Form(Guid? id = null)
    {
      var clusters = await _clusterService.Search();

      var data = id == null
        ? new Identity()
        : await _identityService.GetIdentityByID(id.Value);

      if (data == null)
      {
        throw new NotFoundException();
      }

      return View(new FormModel
      {
        Data = data,
        Clusters = clusters
      });
    }

    [HttpPost]
    [Route("api/[controller]/form")]
    [PermissionFilter(Permission.UserReadWrite)]
    public async Task<Identity> PostForm([FromBody] Identity identity)
    {
      return await _identityService.CreateUser(identity);
    }

    [HttpPut]
    [Route("api/[controller]/form")]
    [PermissionFilter(Permission.UserReadWrite)]
    public async Task<Identity> PutForm([FromBody] Identity identity)
    {
      return await _identityService.UpdateUser(identity);
    }

    [HttpGet]
    [Route("api/[controller]/index")]
    public async Task<IList<Identity>> IndexData(string name = null,
      string email = null,
      [FromQuery] Permission[] permissions = null,
      [FromQuery] long[] clusters = null,
      [FromQuery] RequestType[] requestTypes = null,
      [FromQuery] IdentityStatus status = IdentityStatus.Active)
    {
      return await _identityService.List(new IdentityFilter
      {
        Name = name,
        Email = email,
        Permissions = permissions,
        Clusters = clusters,
        RequestTypes = requestTypes,
        Status = status
      });
    }

    [HttpDelete]
    [Route("api/[controller]/{id}/password")]
    [PermissionFilter(Permission.UserReadWrite)]
    public async Task<string> ResetPassword(Guid id)
    {
      await _identityService.ResetPassword(id);
      return "Ok";
    }
  }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IdentityController : ControllerBase
  {
    readonly IIdentityService _identityservice;

    public IdentityController(IIdentityService identityService)
    {
      _identityservice = identityService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Model.Identity>> List(
      string name = null,
      string email = null,
      [FromQuery] Guid[] ids = null,
      [FromQuery] RequestType[] requestTypes = null,
      [FromQuery] Permission[] permissions = null,
      [FromQuery] long[] clusters = null,
      [FromQuery] string[] nodes = null,
      [FromQuery] IdentityStatus status = IdentityStatus.Active)
    {
      return await _identityservice.List(new IdentityFilter
      {
        Name = name,
        Email = email,
        IDs = ids,
        RequestTypes = requestTypes,
        Permissions = permissions,
        Clusters = clusters,
        Nodes = nodes,
        Status = status
      });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Model.Identity> Get(Guid id)
    {
      return await _identityservice.GetIdentityByID(id);
    }

    [HttpPost]
    public async Task<Model.Identity> Post([FromBody] Model.Identity identity, Guid userID,
      string userName)
    {
      return await _identityservice.CreateIdentity(identity, new Officer(userID, userName));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<Model.Identity> Put(Guid id, [FromBody] Model.Identity identity, Guid userID,
      string userName)
    {
      identity.ID = id;
      return await _identityservice.UpdateIdentity(identity, new Officer(userID, userName));
    }

    [HttpDelete]
    [Route("{id}/password")]
    public async Task<string> ResetPassword(Guid id, Guid userID, string userName)
    {
      await _identityservice.ResetPassword(id, new Officer(userID, userName));
      return "Ok";
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<string> ForgotPassword(string email)
    {
      await _identityservice.ForgotPassword(email);
      return "Ok";
    }

    [HttpGet]
    [Route("CertAuditor")]
    public async Task<Model.Identity> GetCertAuditor(string ClusterNode,
      string name = null,
      string email = null,
      [FromQuery] Guid[] ids = null,
      [FromQuery] RequestType[] requestTypes = null,
      [FromQuery] Permission[] permissions = null,
      [FromQuery] long[] clusters = null)
    {
      return await _identityservice.GetCertificateAuditorToAssign(new IdentityFilter
      {
        Name = name,
        Email = email,
        IDs = ids,
        RequestTypes = requestTypes,
        Permissions = permissions,
        Clusters = clusters
      }, ClusterNode);
    }
  }
}

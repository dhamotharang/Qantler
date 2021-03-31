using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ComplianceController : ControllerBase
  {
    readonly IComplianceService _complianceservice;

    public ComplianceController(IComplianceService complianceservice)
    {
      _complianceservice = complianceservice;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Model.ComplianceHistory>> Get(Scheme scheme)
    {
      return await _complianceservice.GetComplianceByScheme(scheme);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<Model.ComplianceHistory> Get(
      long id)
    {
      return await _complianceservice.GetComplianceByID(id);
    }

  }
}
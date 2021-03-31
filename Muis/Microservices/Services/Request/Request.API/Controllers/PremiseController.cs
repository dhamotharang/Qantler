using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PremiseController : Controller
  {
    readonly IPremiseService _premiseService;

    public PremiseController(IPremiseService premiseService)
    {
      _premiseService = premiseService;
    }

    [HttpGet]
    [Route("query")]
    public async Task<IList<Premise>> Query(Guid? customerID)
    {
      return await _premiseService.GetPremises(customerID);
    }

    [HttpPost]
    [Route("")]
    public async Task<Premise> CreatePremises(Premise data)
    {
      return await _premiseService.CreatePremise(data);
    }
  }
}

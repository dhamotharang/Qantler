using eHS.Portal.Model;
using eHS.Portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  public class PremiseController : Controller
  {
    readonly IPremiseService _premiseService;

    public PremiseController(IPremiseService premiseService)
    {
      _premiseService = premiseService;
    }

    [Route("/api/[controller]/list")]
    [HttpGet]
    public async Task<IList<Premise>> ListPremise(Guid? customerID)
    {
      return await _premiseService.GetPremise(customerID);
    }

    [Route("/api/[controller]")]
    [HttpPost]
    public async Task<Premise> CreatePremise([FromBody] Premise premise)
    {
      return await _premiseService.CreatePremise(premise);
    }
  }
}

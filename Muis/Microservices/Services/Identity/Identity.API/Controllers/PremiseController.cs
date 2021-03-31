using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PremiseController : ControllerBase
  {
    readonly IPremiseService _premiseService;

    public PremiseController(IPremiseService premiseService)
    {
      _premiseService = premiseService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IList<Premise>> List()
    {
      return await _premiseService.GetPremises();
    }
  }
}
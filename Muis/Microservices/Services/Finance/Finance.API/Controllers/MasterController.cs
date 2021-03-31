using Finance.API.Services;
using Finance.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MasterController : ControllerBase
  {
    readonly IMasterService _masterService;

    public MasterController(IMasterService masterService)
    {
      _masterService = masterService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IEnumerable<Master>> Get(MasterType type)
    {
      return await _masterService.GetMaster(type);
    }
  }
}
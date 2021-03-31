using System.Threading.Tasks;
using JobOrder.Model;
using Microsoft.AspNetCore.Mvc;
using JobOrder.API.Services;

namespace JobOrder.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FindingsController : ControllerBase
  {
    readonly IFindingsService _findingsService;

    public FindingsController(IFindingsService findingsService)
    {
      _findingsService = findingsService;
    }

    [HttpPost]
    [Route("")]
    public async Task<Findings> Post([FromBody] Findings findings)
    {
      return await _findingsService.Submit(findings);
    }
  }
}
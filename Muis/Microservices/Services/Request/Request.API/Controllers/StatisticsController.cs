using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StatisticsController
  {
    readonly IStatisticsService _service;

    public StatisticsController(IStatisticsService service)
    {
      _service = service;
    }

    [HttpGet]
    [Route("performance")]
    public async Task<IList<StatisticsPerformance>> Performance([FromQuery] string[] keys = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Performance(keys?.ToList(), from, to);
    }

    [HttpGet]
    [Route("overview")]
    public async Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Overview(from, to);
    }

    [HttpGet]
    [Route("status")]
    public async Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Status(from, to);
    }
  }
}

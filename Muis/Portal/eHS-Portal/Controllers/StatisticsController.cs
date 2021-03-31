using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Services.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  public class StatisticsController
  {
    readonly IStatisticsService _service;

    public StatisticsController(IStatisticsService service)
    {
      _service = service;
    }

    [HttpGet]
    [Route("api/[controller]/performance")]
    public async Task<IList<StatisticsPerformance>> Performance([FromQuery] string[] keys = null,
      DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Performance(keys?.ToList(), from, to);
    }

    [HttpGet]
    [Route("api/[controller]/overview")]
    public async Task<IList<StatisticsOverview>> Overview(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Overview(from, to);
    }

    [HttpGet]
    [Route("api/[controller]/status")]
    public async Task<IList<StatisticsStatus>> Status(DateTimeOffset? from = null,
      DateTimeOffset? to = null)
    {
      return await _service.Status(from, to);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using JobOrder.API.Services;
using JobOrder.Model;
using Microsoft.AspNetCore.Mvc;

namespace JobOrder.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SettingsController : ControllerBase
  {
    readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
      _settingsService = settingsService;
    }

    [HttpGet]
    public async Task<List<Settings>> GetSettings()
    {
      return await _settingsService.GetJobOrderSettings();
    }

    [HttpPut]
    public async Task<List<Settings>> Put(
      [FromBody] IList<Settings> settings, Guid userID, string userName)
    {
      await _settingsService.UpdateSettings(settings, userID, userName);

      return await _settingsService.GetJobOrderSettings();
    }
  }
}
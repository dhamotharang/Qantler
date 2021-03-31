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
      return await _settingsService.GetRequestSettings();
    }

    [HttpPut]
    public async Task<string> Put([FromBody] IList<Settings> settings, Guid userID, string userName)
    {
      await _settingsService.UpdateSettings(settings, userID, userName);

      return "OK";
    }
  }

}

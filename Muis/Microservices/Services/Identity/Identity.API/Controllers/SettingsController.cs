using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Identity.API.Services;
using Identity.Model;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SettingsController : ControllerBase
  {
    readonly ISettingService _settingsService;

    public SettingsController(ISettingService settingsService)
    {
      _settingsService = settingsService;
    }

    [HttpGet]
    public async Task<List<Settings>> GetSettings()
    {
      return await _settingsService.GetIdentitySettings();
    }

    [HttpPut]
    public async Task<string> Put([FromBody] IList<Settings> settings, Guid userID, string userName)
    {
      await _settingsService.UpdateSettings(settings, userID, userName);

      return "OK";
    }
  }
}

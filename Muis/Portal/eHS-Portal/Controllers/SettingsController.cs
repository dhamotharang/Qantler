using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Filters;
using eHS.Portal.Model;
using eHS.Portal.Models.Settings;
using eHS.Portal.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eHS.Portal.Controllers
{
  [Authorize]
  [ServiceFilter(typeof(SessionAwareFilter))]
  [PermissionFilter(Permission.SystemRead,
      Permission.SystemReadWrite)]
  public class SettingsController : Controller
  {
    readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
      _settingsService = settingsService;
    }

    [Route("[controller]")]
    public async Task<IActionResult> Settings()
    {
      return View(new SettingsModel
      {
        Data = await _settingsService.GetSystemSettings()
      });
    }

    [HttpPut]
    [Route("api/[controller]/systemConfig")]
    [PermissionFilter(Permission.SystemReadWrite)]
    public async Task<string> Put([FromBody] SettingsDTO settings)
    {
      await _settingsService.UpdateSettings(settings);

      return "success";
    }
  }
}
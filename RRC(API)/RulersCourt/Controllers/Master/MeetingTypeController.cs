using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;

namespace RulersCourt.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class MeetingTypeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public MeetingTypeController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Meeting")]
        public IActionResult GetLocation()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_MeetingClient>.Instance.GetMeetingType(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
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
    public class M_MediaChannelController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_MediaChannelController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("MediaChannel")]
        public IActionResult GetMediaChannel()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_MediaChannelClient>.Instance.GetMediaChannel(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
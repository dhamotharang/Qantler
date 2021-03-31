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
    public class M_ReligionController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_ReligionController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Religion")]
        public IActionResult GetReligion()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_ReligionClient>.Instance.GetReligion(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
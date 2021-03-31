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
    public class M_OfficialTaskRequestController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_OfficialTaskRequestController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("OfficialTaskRequest")]
        public IActionResult GetOfficialTaskRequest()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_OfficialTaskRequestClient>.Instance.GetOfficialTaskRequest(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
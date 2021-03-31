using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository.Master;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_LeaveTypeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_LeaveTypeController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("LeaveType")]
        public IActionResult GetLeaveType()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_LeaveTypeClient>.Instance.GetLeaveType(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
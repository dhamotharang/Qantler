using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository.Master.Vehicle;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_VehicleIssuesController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_VehicleIssuesController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("VehicleIssues")]
        public IActionResult GetVehicleIssues()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_VehicleissueClient>.Instance.GetVehicleIssues(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}

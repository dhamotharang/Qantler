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
    public class M_VehicleRequestStatusController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_VehicleRequestStatusController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("VehicleRequestStatus")]
        public IActionResult GetTripType()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_VehicleRequestStatusClient>.Instance.GetVehicleRequestStatus(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}

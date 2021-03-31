using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_TripReleaseLocationController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_TripReleaseLocationController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("TripReleaseLocation")]
        public IActionResult GetTripDestination()
        {
            M_TripReleaseLocationModel res = new M_TripReleaseLocationModel();
            return Ok(res);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository.Master.Vehicle;
using System;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_TripDestinationController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_TripDestinationController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("TripDestination")]
        public IActionResult GetTripDestination()
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<M_TripDestinationClient>.Instance.GetTripDestination(appSettings.Value.ConnectionString, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}

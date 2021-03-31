using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using System;

namespace RulersCourt.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_LocationController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_LocationController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Location")]
        public IActionResult GetLocation()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_LocationClient>.Instance.GetLocation(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
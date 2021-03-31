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
    public class M_EventTypeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_EventTypeController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Event")]
        public IActionResult GetEventType()
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<M_EventTypeClient>.Instance.GetEventType(appSettings.Value.ConnectionString, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}

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
    public class M_SpecializationController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_SpecializationController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Specialization")]
        public IActionResult GetSpecialization()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_SpecializationClient>.Instance.GetSpecialization(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}
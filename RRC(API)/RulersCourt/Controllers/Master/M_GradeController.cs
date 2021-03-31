using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;

namespace RulersCourt.Controllers.Master
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_GradeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_GradeController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Grade")]
        public IActionResult GetGrade()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_GradeClient>.Instance.GetGrade(appSettings.Value.ConnectionString, userID, lang);
            return Ok(result);
        }
    }
}

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
    public class M_CityController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_CityController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("City")]
        public IActionResult GetCity()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            string country = Request.Query["CountryID"];
            string emirates = Request.Query["EmiratesID"];
            var result = DbClientFactory<M_CityClient>.Instance.GetCity(appSettings.Value.ConnectionString, userID, country, lang, emirates);
            return Ok(result);
        }
    }
}
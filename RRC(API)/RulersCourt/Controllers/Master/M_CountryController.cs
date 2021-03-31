using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using System.Web;

namespace RulersCourt.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class M_CountryController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;

        public M_CountryController(IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("Country")]
        public IActionResult GetCountry()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var module = parameters["Module"];
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<M_CountryClient>.Instance.GetCountry(appSettings.Value.ConnectionString, userID, lang, module);
            return Ok(result);
        }
    }
}
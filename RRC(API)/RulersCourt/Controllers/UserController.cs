using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using RulersCourt.Services;
using System;
using System.Collections.Generic;
using System.Web;

namespace RulersCourt.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IAuthenticationService authService;
        private readonly IOptions<WrdUserLoginCredentialsModel> wrdUserLoginCredentials;

        public UserController(IOptions<ConnectionSettingsModel> app, IAuthenticationService authSvc, IOptions<WrdUserLoginCredentialsModel> wrdUser)
        {
            appSettings = app;
            authService = authSvc;
            wrdUserLoginCredentials = wrdUser;
        }

        [HttpPost]
        [Route("User")]
        public IActionResult GetUser([FromBody]List<OrganizationModel> value)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            int paramType = string.IsNullOrEmpty(parameters["Type"]) ? 0 : Convert.ToInt32(parameters["Type"]);
            string department = string.Join(",", value.ConvertAll(x => x.OrganizationID).ToArray());
            var result = DbClientFactory<UserClient>.Instance.GetUsers(appSettings.Value.ConnectionString, department, paramUserID, paramType, lang);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("User/Login")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var user = authService.Authenticate(userParam.Username, userParam.Password, wrdUserLoginCredentials.Value);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            var result = DbClientFactory<UserClient>.Instance.GetLoginUsers(appSettings.Value.ConnectionString, user, lang);
            return Ok(result);
        }
    }
}
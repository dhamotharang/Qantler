using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using System;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.General
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]

    public class GeneralHomeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public GeneralHomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("General/Home/AllModulesPendingCount/{UserID:int}")]
        public IActionResult GetAllModulesPendingTasksCount()
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var response = DbClientFactory<HomeClient>.Instance.GetModulesCount(appSettings.Value.ConnectionString, paramUserID, lang);
            return Ok(response);
        }
    }
}
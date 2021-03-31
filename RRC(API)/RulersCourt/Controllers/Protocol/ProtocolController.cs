using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository.Protocol;
using System;
using Workflow.Interface;

namespace RulersCourt.Controllers.Protocol
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class ProtocolController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public ProtocolController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Protocol/Home/AllModulesPendingCount/{UserID:int}")]
        public IActionResult GetAllModulesPendingTasksCount(int userID)
        {
            var response = DbClientFactory<ProtocolHomeClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }
    }
}
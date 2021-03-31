using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository;
using System;
using System.Collections.Generic;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Route("api/")]
    [Authorize]
    public class ShareParticipationUsersController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public ShareParticipationUsersController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("ShareParticipation/{ServiceID:int}")]
        public IActionResult GetAllShareParticipation(int serviceID)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var result = DbClientFactory<ShareparticipationUsersClient>.Instance.GetparticipationUsers(appSettings.Value.ConnectionString, serviceID);
            return Ok(result);
        }

        [HttpPost]
        [Route("ShareParticipation/{ServiceID:int}")]
        public IActionResult SaveShareParticipation(int serviceID, [FromBody]List<ShareparticipationUsersModel> participation)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var type = Request.Query["Type"];
            var userID = int.Parse(Request.Query["UserID"]);
            string comments = Request.Query["Comments"];
            var result = DbClientFactory<ShareparticipationUsersClient>.Instance.SaveparticipationUsers(appSettings.Value.ConnectionString, participation, serviceID, type, userID, comments);
            Workflow.WorkflowBO bo = new MemoGetWorkflow().GetMemoWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = serviceID;
            workflow.StartWorkflow(bo);
            return Ok(serviceID);
        }
    }
}
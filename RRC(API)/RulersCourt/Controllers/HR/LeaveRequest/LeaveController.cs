using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Controllers.LeaveRequest;
using RulersCourt.Models;
using RulersCourt.Models.LeaveRequest;
using RulersCourt.Repository;
using RulersCourt.Repository.Leave;
using System;
using System.Threading.Tasks;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class LeaveController : LeaveCommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public LeaveController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Leave/{id}")]
        public IActionResult GetLeaveByID(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<LeaveClient>.Instance.GetLeaveByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Leave")]
        public IActionResult SaveLeave([FromBody]LeavePostModel leave)
        {
            var result = DbClientFactory<LeaveClient>.Instance.PostLeave(appSettings.Value.ConnectionString, leave);
            LeaveSaveResponseModel res = new LeaveSaveResponseModel();
            res.LeaveID = result.LeaveID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new LeaveGetWorkflow().GetLeaveWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LeaveID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Leave")]
        public IActionResult UpdateLeave([FromBody]LeavePutModel leave)
        {
            var result = DbClientFactory<LeaveClient>.Instance.PutLeave(appSettings.Value.ConnectionString, leave);
            LeaveSaveResponseModel res = new LeaveSaveResponseModel();
            res.LeaveID = result.LeaveID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new LeaveGetWorkflow().GetLeaveWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LeaveID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpDelete]
        [Route("Leave/{id:int}")]
        public IActionResult DeleteLeave(int id)
        {
            var result = DbClientFactory<LeaveClient>.Instance.DeleteLeave(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPatch]
        [Route("Leave/{id}")]
        public IActionResult ModifyLeave(int id, [FromBody]JsonPatchDocument<LeavePutModel> value)
        {
            var result = DbClientFactory<LeaveClient>.Instance.PatchLeave(appSettings.Value.ConnectionString, id, value);
            LeaveSaveResponseModel res = new LeaveSaveResponseModel();
            res.LeaveID = result.LeaveID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            res.HRManagerUserID = result.HRManagerUserID;
            Workflow.WorkflowBO bo = new LeaveGetWorkflow().GetLeaveWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.LeaveID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("Leave/Report")]
        public async Task<IActionResult> Export([FromBody]LeaveReportRequestModel report, string type)
        {
            try
            {
                var result = DbClientFactory<LeaveReportClient>.Instance.GetLeaveReporExporttList(appSettings.Value.ConnectionString, report);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Leave/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]LeaveCommunicationHistory value)
        {
            var result = DbClientFactory<LeaveClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}
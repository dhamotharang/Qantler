using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Maintenance;
using RulersCourt.Repository.Maintenance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class MaintenanceController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public MaintenanceController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Maintenance/{id}")]
        public IActionResult GetMaintenanceByID(int id)
        {
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MaintenanceClient>.Instance.GetMaintenanceByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Maintenance")]
        public IActionResult SaveMaintenance([FromBody]MaintenancePostModel maintenance)
        {
            MaintenanceSaveResponseModel response = new MaintenanceSaveResponseModel();
            var result = DbClientFactory<MaintenanceClient>.Instance.PostMaintenance(appSettings.Value.ConnectionString, maintenance);
            response.MaintenanceID = result.MaintenanceID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetMaintenanceWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.MaintenanceID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPut]
        [Route("Maintenance")]
        public IActionResult UpdateMaintenance([FromBody]MaintenancePutModel maintenance)
        {
            MaintenanceSaveResponseModel response = new MaintenanceSaveResponseModel();
            var result = DbClientFactory<MaintenanceClient>.Instance.PutMaintenance(appSettings.Value.ConnectionString, maintenance);
            response.MaintenanceID = result.MaintenanceID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new GetMaintenanceWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.MaintenanceID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpDelete]
        [Route("Maintenance/{id:int}")]
        public IActionResult DeleteMaintenance(int id)
        {
            var result = DbClientFactory<MaintenanceClient>.Instance.DeleteMaintenance(appSettings.Value.ConnectionString, id);
            return Ok(result);
        }

        [HttpPut]
        [Route("Maintenance/Attachment/{id:int}")]
        public IActionResult UpdateMaintenanceAttachment(int id, [FromBody]List<MaintenanceAttachmentGetModel> attachments)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            DbClientFactory<MaintenanceClient>.Instance.PutMaintenanceAttachments(appSettings.Value.ConnectionString, attachments, id);
            return Ok(id);
        }

        [HttpPatch]
        [Route("Maintenance/{id}")]
        public IActionResult ModifyMaintenance(int id, [FromBody]JsonPatchDocument<MaintenancePutModel> value)
        {
            MaintenanceSaveResponseModel response = new MaintenanceSaveResponseModel();
            var result = DbClientFactory<MaintenanceClient>.Instance.PatchMaintenance(appSettings.Value.ConnectionString, id, value);
            response.MaintenanceID = result.MaintenanceID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            response.MaintenanceManagerUserID = result.MaintenanceManagerUserID;
            Workflow.WorkflowBO bo = new GetMaintenanceWorkflow().GetWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = response.MaintenanceID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(response);
        }

        [HttpPost]
        [Route("Maintenance/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]MaintenanceCommunicationHistory value)
        {
            var result = DbClientFactory<MaintenanceClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }

        [HttpPost]
        [Route("Maintenance/Report")]
        public async Task<IActionResult> Export([FromBody]MaintenanceReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<MaintenanceClient>.Instance.GetReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
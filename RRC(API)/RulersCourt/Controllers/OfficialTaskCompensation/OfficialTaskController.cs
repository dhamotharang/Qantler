using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.OfficalTask;
using RulersCourt.Models.OfficialTask;
using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using RulersCourt.Repository.OfficialTaskCompensation.OfficalTask;
using RulersCourt.Repository.OfficialTaskCompensation.OfficialTask;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.OfficalTaskCompensation
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class OfficialTaskController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;

        public OfficialTaskController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            environment = env;
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("OfficialTask/{id}")]
        public IActionResult GetOfficialTaskByID(int id)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                int userID = int.Parse(Request.Query["UserID"]);
                var result = DbClientFactory<OfficialTaskClient>.Instance.GetOfficialTaskByID(appSettings.Value.ConnectionString, id, userID, lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("OfficialTask")]
        public IActionResult SaveOfficialTask([FromBody]OfficialTaskPostModel officialTask)
        {
            try
            {
                var result = DbClientFactory<OfficialTaskClient>.Instance.PostOfficialTask(appSettings.Value.ConnectionString, officialTask);
                OfficialTaskSaveResponseModel res = new OfficialTaskSaveResponseModel();
                res.OfficialTaskID = result.OfficialTaskID;
                res.ReferenceNumber = result.ReferenceNumber;
                res.Status = result.Status;
                Workflow.WorkflowBO bo = new OfficialTaskGetWorkflow().GetOfficialTaskWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = res.OfficialTaskID ?? 0;
                workflow.StartWorkflow(bo);
                return Ok(res);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPut]
        [Route("OfficialTask")]
        public IActionResult UpdateOfficialTask([FromBody]OfficialTaskPutModel officialTask)
        {
            var result = DbClientFactory<OfficialTaskClient>.Instance.PutOfficialTask(appSettings.Value.ConnectionString, officialTask);
            OfficialTaskSaveResponseModel res = new OfficialTaskSaveResponseModel();
            res.OfficialTaskID = result.OfficialTaskID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new OfficialTaskGetWorkflow().GetOfficialTaskWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.OfficialTaskID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("OfficialTask/{id}")]
        public IActionResult ModifyOfficialTask(int id, [FromBody]JsonPatchDocument<OfficialTaskPutModel> value)
        {
            var result = DbClientFactory<OfficialTaskClient>.Instance.PatchOfficialTask(appSettings.Value.ConnectionString, id, value);
            OfficialTaskSaveResponseModel res = new OfficialTaskSaveResponseModel();
            res.OfficialTaskID = result.OfficialTaskID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new OfficialTaskGetWorkflow().GetOfficialTaskWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.OfficialTaskID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPost]
        [Route("OfficialTask/Report")]
        public async Task<IActionResult> Export([FromBody]OfficialTaskReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<OfficialTaskReportClient>.Instance.GetReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("OfficialTask/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]OfficialTaskCommunicationHistoryModel value)
        {
            var result = DbClientFactory<OfficialTaskClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }

        [HttpPost]
        [Route("OfficialTask/User")]
        public IActionResult GetOfficialTaskUser([FromBody]List<OrganizationModel> value)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["StartDate"]) ? (DateTime?)null : DateTime.Parse(parameters["StartDate"]);
            DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["EndDate"]) ? (DateTime?)null : DateTime.Parse(parameters["EndDate"]);

            string department = string.Join(",", value.ConvertAll(x => x.OrganizationID).ToArray());
            var result = DbClientFactory<OfficialTaskClient>.Instance.GetOfficialTaskUser(appSettings.Value.ConnectionString, department, paramUserID, paramReqDateFrom, paramReqDateTo);
            return Ok(result);
        }

        [HttpPost]
        [Route("OfficialTask/UserAvailability")]
        public IActionResult UserAvailability([FromBody] List<OfficialTaskEmployeeNameModel> employee)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramStartDate = parameters["StartDate"];
                var paramUserID = parameters["UserID"];
                var paramEndDate = parameters["EndDate"];
                var result = DbClientFactory<OfficialTaskClient>.Instance.UserAvailability(appSettings.Value.ConnectionString, paramUserID, paramStartDate, paramEndDate, employee);
                return Ok(result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}

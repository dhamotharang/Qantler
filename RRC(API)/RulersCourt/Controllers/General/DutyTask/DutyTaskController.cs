using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.DutyTask;
using RulersCourt.Repository.DutyTask;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.DutyTask
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class DutyTaskController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public DutyTaskController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("DutyTask/{id}")]
        public IActionResult GetTaskByID(int id)
        {
            var userID = int.Parse(Request.Query["UserID"]);
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var response = DbClientFactory<DutyTaskClient>.Instance.GetDutyTaskByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(response);
        }

        [HttpPost]
        [Route("DutyTask")]
        public IActionResult SaveTask([FromBody]DutyTaskPostModel dutyTask)
        {
            DutyTaskSaveResponseModel res = new DutyTaskSaveResponseModel();
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<DutyTaskClient>.Instance.PostDutyTask(appSettings.Value.ConnectionString, dutyTask, lang);
            res.TaskID = result.TaskID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DutyTaskGetWorkflow().GetDutyTaskWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.TaskID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("DutyTask")]
        public IActionResult UpdateTask([FromBody]DutyTaskPutModel dutyTask)
        {
            DutyTaskSaveResponseModel res = new DutyTaskSaveResponseModel();
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<DutyTaskClient>.Instance.PutDutyTask(appSettings.Value.ConnectionString, dutyTask, lang);
            res.TaskID = result.TaskID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            if (dutyTask.Action != "Update")
            {
                Workflow.WorkflowBO bo = new DutyTaskGetWorkflow().GetDutyTaskWorkflow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = res.TaskID ?? 0;
                workflow.StartWorkflow(bo);
            }

            return Ok(res);
        }

        [HttpDelete]
        [Route("DutyTask/{id:int}")]
        public IActionResult DeleteTask(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<DutyTaskClient>.Instance.DeleteDutyTask(appSettings.Value.ConnectionString, id, userID, lang);
            Workflow.WorkflowBO bo = new DutyTaskGetWorkflow().GetDutyTaskWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.TaskID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(result);
        }

        [HttpPatch]
        [Route("DutyTask/{id}")]
        public IActionResult ModifyMemo(int id, [FromBody]JsonPatchDocument<DutyTaskPutModel> dutyTask)
        {
            DutyTaskSaveResponseModel res = new DutyTaskSaveResponseModel();
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<DutyTaskClient>.Instance.PatchDutyTask(appSettings.Value.ConnectionString, id, dutyTask, lang);
            res.TaskID = result.TaskID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new DutyTaskGetWorkflow().GetDutyTaskWorkflow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = res.TaskID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpGet]
        [Route("DutyTask/Letters/{UserID}")]
        public async Task<IActionResult> M_Letters(int userID)
        {
            var referenceNumber = Request.Query["ReferenceNumber"];
            var result = await DbClientFactory<DutyTaskClient>.Instance.GetLinkToLetter(appSettings.Value.ConnectionString, userID, referenceNumber);
            return Ok(result);
        }

        [HttpGet]
        [Route("DutyTask/Memos/{UserID}")]
        public async Task<IActionResult> M_Memos(int userID)
        {
            var referenceNumber = Request.Query["ReferenceNumber"];
            var result = await DbClientFactory<DutyTaskClient>.Instance.GetLinkToMemo(appSettings.Value.ConnectionString, userID, referenceNumber);
            return Ok(result);
        }

        [HttpGet]
        [Route("DutyTask/Meetings/{UserID}")]
        public async Task<IActionResult> M_Meetings(int userID)
        {
            var referenceNumber = Request.Query["ReferenceNumber"];
            var result = await DbClientFactory<DutyTaskClient>.Instance.GetLinkToMeeting(appSettings.Value.ConnectionString, userID, referenceNumber);
            return Ok(result);
        }

        [HttpGet]
        [Route("DutyTask/GetCountryAndEmirates")]
        public IActionResult GetCountryEmirates()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var module = parameters["Module"];
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<DutyTaskClient>.Instance.GetCountryAndEmirates(appSettings.Value.ConnectionString, userID, lang, module);
            return Ok(result);
        }

        [HttpPost]
        [Route("DutyTask/CommunicationHistory")]
        public IActionResult CommunicationChat([FromBody]DutyTaskCommunicationHistoryModel value)
        {
            var result = DbClientFactory<DutyTaskClient>.Instance.SaveCommunicationHistory(appSettings.Value.ConnectionString, value);
            if (value.TaggedUserID.Count > 0)
            {
                Workflow.WorkflowBO bo = new DutyTaskGetWorkflow().GetDutyTaskCommunicationWorkflow(value, appSettings.Value.ConnectionString);
                bo.ServiceID = value.TaskID ?? 0;
                workflow.StartWorkflow(bo);
            }

            return Ok(result);
        }
    }
}
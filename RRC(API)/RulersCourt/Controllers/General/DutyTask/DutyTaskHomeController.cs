using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
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
    public class DutyTaskHomeController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public DutyTaskHomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("DutyTasks/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllTasks(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUserID = parameters["UserID"];
            var paramStatus = parameters["Status"];
            var paramCreator = parameters["Creator"];
            var paramAssignee = parameters["Assignee"];
            var paramPriority = parameters["Priority"];
            var paramLable = parameters["Lable"];
            var paramLinkTo = parameters["LinkTo"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            DateTime? paramDueDateFrom = string.IsNullOrEmpty(parameters["DueDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DueDateFrom"]);
            DateTime? paramDueDateTo = string.IsNullOrEmpty(parameters["DueDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DueDateTo"]);
            DateTime? paramCreationDateFrom = string.IsNullOrEmpty(parameters["CreationDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["CreationDateFrom"]);
            DateTime? paramCreationDateTo = string.IsNullOrEmpty(parameters["CreationDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["CreationDateTo"]);
            var paramParticipants = parameters["Participants"];
            var paramSmartSearch = parameters["SmartSearch"];
            var response = DbClientFactory<DutyTaskClient>.Instance.GetDutyTasks(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramStatus, paramCreator, paramAssignee, paramPriority, paramLable, paramLinkTo, paramDueDateFrom, paramDueDateTo, paramCreationDateFrom, paramCreationDateTo, paramParticipants, paramSmartSearch, lang);
            return Ok(response);
        }

        [HttpGet]
        [Route("DutyTasks/Home/Count/{UserID:int}")]
        public async Task<IActionResult> GetHomeCount(int userID)
        {
            var response = await DbClientFactory<DutyTaskClient>.Instance.GetHomeCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("DutyTasks/Report")]
        public async Task<IActionResult> Export([FromBody]DutyTaskReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<DutyTaskClient>.Instance.GetDutyTasksReport(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
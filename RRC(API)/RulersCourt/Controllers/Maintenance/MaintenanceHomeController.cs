using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Repository.Maintenance;
using System;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.HR
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class MaintenanceHomeController : ControllerBase
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public MaintenanceHomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Maintenance/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllTasks(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramStatus = parameters["Status"];
            var paramUserID = parameters["UserID"];
            var paramSourceOU = parameters["SourceOU"];
            var paramstringUserName = parameters["UserName"];
            var paramSourceName = parameters["SourceName"];
            var paramSubject = parameters["Subject"];
            var paramAttendedBy = parameters["AttendedBy"];
            DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
            DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
            var paramPriority = parameters["Priority"];
            var paramSmartSearch = parameters["SmartSearch"];
            var paramType = parameters["Type"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MaintenanceClient>.Instance.GetMaintenance(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramStatus, paramSourceOU, paramSubject, paramstringUserName, paramPriority, paramAttendedBy, paramReqDateFrom, paramReqDateTo, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Maintenance/Home/Count/{UserID:int}")]
        public IActionResult GetHomeCount(int userID)
        {
            var response = DbClientFactory<MaintenanceClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("Maintenance/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<MaintenanceClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("Maintenance/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<MaintenanceClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Maintenance/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<MaintenanceClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Maintenance/Document/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetDocument(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = int.Parse(parameters["UserID"]);
            var paramType = parameters["Type"];
            var paramCreator = parameters["Creator"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<MaintenanceClient>.Instance.GetDocument(appSettings.Value.ConnectionString, paramUserID, paramType, pageNumber, pageSize, paramCreator, paramSmartSearch, lang);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.HR;
using RulersCourt.Repository;
using RulersCourt.Repository.HR;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.HR
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class HRController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public HRController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("HR/Home/AllModulesPending/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllModulesPendingTasks(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramStatus = parameters["Status"];
                var paramRequestType = parameters["RequestType"];
                var paramUserID = parameters["UserID"];
                var paramUserName = parameters["UserName"];
                var paramCreator = parameters["Creator"];
                DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
                DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var response = DbClientFactory<HRHomeClient>.Instance.GetAllHRModules(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserName, paramUserID, paramCreator, paramStatus, paramReqDateFrom, paramReqDateTo, paramSmartSearch, lang);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("HR/Home/AllModulesPendingCount/{UserID:int}")]
        public IActionResult GetAllModulesPendingTasksCount(int userID)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramRequestType = parameters["RequestType"];
                var paramUserName = parameters["UserName"];
                var response = DbClientFactory<HRHomeClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("HR/Home/MyPending/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetMyPendingTasks(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramStatus = parameters["Status"];
                var paramRequestType = parameters["RequestType"];
                var paramUserID = parameters["UserID"];
                var paramUserName = parameters["UserName"];
                var paramCreator = parameters["Creator"];
                DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
                DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var response = DbClientFactory<HRHomeClient>.Instance.GetMyPending(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserName, paramUserID, paramCreator, paramStatus, paramReqDateFrom, paramReqDateTo, paramSmartSearch, lang);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("HR/Home/MyProcessed/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetMyProcessedTasks(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramStatus = parameters["Status"];
                var paramRequestType = parameters["RequestType"];
                var paramUserID = parameters["UserID"];
                var paramUserName = parameters["UserName"];
                var paramCreator = parameters["Creator"];
                DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
                DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var response = DbClientFactory<HRHomeClient>.Instance.GetMyProcessed(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserName, paramUserID, paramCreator, paramStatus, paramReqDateFrom, paramReqDateTo, paramSmartSearch, lang);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet]
        [Route("HR/Home/MyOwnRequest/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetMyOwnRequest(int pageNumber, int pageSize)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramUserID = parameters["UserID"];
                var paramStatus = parameters["Status"];
                var paramRequestType = parameters["RequestType"];
                var paramUserName = parameters["UserName"];
                var paramCreator = parameters["Creator"];
                DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
                DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
                var paramSmartSearch = parameters["SmartSearch"];
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var response = DbClientFactory<HRHomeClient>.Instance.GetMyOwnRequest(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserName, paramUserID, paramCreator, paramStatus, paramReqDateFrom, paramReqDateTo, paramSmartSearch, lang);
                return Ok(response);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("HR/Report")]
        public async Task<IActionResult> Export([FromBody]HRReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<HRReportClient>.Instance.GetHRReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("HR/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("HR/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("HR/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<DocumentClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("HR/Document/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetDocument(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = int.Parse(parameters["UserID"]);
            var paramType = parameters["Type"];
            var paramCreator = parameters["Creator"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<DocumentClient>.Instance.GetDocument(appSettings.Value.ConnectionString, paramUserID, paramType, pageNumber, pageSize, paramCreator, paramSmartSearch, lang);
            return Ok(result);
        }
    }
}
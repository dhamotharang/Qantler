using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.ITSupport;
using RulersCourt.Repository;
using RulersCourt.Repository.ITSupportClient;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.ITSupport
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class ITSupportHomeController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public ITSupportHomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("ITSupport/Home/List/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllTasks(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramUserName = parameters["UserName"];
            var paramRequestType = parameters["RequestType"];
            var paramType = parameters["Type"];
            var paramSourceOU = parameters["SourceOU"];
            var paramSubject = parameters["Subject"];
            var paramPriority = parameters["Priority"];
            var paramStatus = parameters["Status"];
            DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
            DateTime? paramRequestDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var response = DbClientFactory<ITSupportHomeClient>.Instance.GetITSupport(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserID, paramUserName, paramSourceOU, paramSubject, paramStatus, paramPriority, paramReqDateFrom, paramRequestDateTo, paramSmartSearch, lang, paramType);
            return Ok(response);
        }

        [HttpGet]
        [Route("ITSupport/Home/Count/{UserID:int}")]
        public IActionResult GetHomeCount(int userID)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var response = DbClientFactory<ITSupportHomeClient>.Instance.GetAllTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("ITSupport/Report")]
        public async Task<IActionResult> Export([FromBody]ITSupportReportModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<ITSupportHomeClient>.Instance.GetITSupportReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("ITSupport/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("ITSupport/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("ITSupport/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<DocumentClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("ITSupport/Document/{PageNumber:int},{PageSize:int}")]
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
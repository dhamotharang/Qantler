using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Protocol.Media;
using RulersCourt.Repository;
using RulersCourt.Repository.Protocol.Media;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Protocol.Media
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    [Route("api/")]
    public class MediaHomeController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public MediaHomeController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Media/Home/AllModulesPending/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllModulesPendingTasks(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramStatus = parameters["Status"];
            var paramRequestType = parameters["RequestType"];
            var paramType = parameters["Type"];
            var paramUserID = parameters["UserID"];
            var paramSourceOU = parameters["SourceOU"];
            if (!string.IsNullOrEmpty(paramSourceOU) & !string.IsNullOrEmpty(paramSourceOU))
            {
                paramSourceOU = paramSourceOU.Replace("amp;", "&");
            }

            var paramSmartSearch = parameters["SmartSearch"];
            DateTime? paramReqDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
            DateTime? paramReqDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
            var response = DbClientFactory<MediaHomeClient>.Instance.GetAllMediaModules(appSettings.Value.ConnectionString, pageNumber, pageSize, paramRequestType, paramUserID, paramStatus, paramReqDateFrom, paramReqDateTo, paramSmartSearch, paramSourceOU, paramType, lang);
            return Ok(response);
        }

        [HttpGet]
        [Route("Media/Home/AllModulesPendingCount/{UserID:int}")]
        public IActionResult GetAllModulesPendingTasksCount(int userID)
        {
            var response = DbClientFactory<MediaHomeClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("Media/export")]
        public async Task<IActionResult> Export([FromBody]MediaExportModel report, string type)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MediaExportClient>.Instance.GetMediaReportExportList(appSettings.Value.ConnectionString, report, lang);
            return await this.Export(type, result);
        }

        [HttpPost]
        [Route("Media/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("Media/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<DocumentClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Media/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<DocumentClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Media/Document/{PageNumber:int},{PageSize:int}")]
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
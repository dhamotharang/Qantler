using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Legal;
using RulersCourt.Repository.Legal;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Legal
{
    [EnableCors("AllowOrigin")]
    [Produces("application/json")]
    [Authorize]
    public class LegalHomePageController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IHostingEnvironment environment;
        private readonly IWorkflowClient workflow;

        public LegalHomePageController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [Route("Legal/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetAllLegal(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramType = parameters["Type"];
            var paramUserID = parameters["UserID"];
            var paramStatus = parameters["Status"];
            var paramUserName = parameters["UserName"];
            var paramSourceOU = parameters["SourceOU"];
            if (!string.IsNullOrEmpty(paramSourceOU) & !string.IsNullOrEmpty(paramSourceOU))
            {
                paramSourceOU = paramSourceOU.Replace("amp;", "&");
            }

            var paramSubject = parameters["Subject"];
            var paramLabel = parameters["Label"];
            var paramAttendedBy = parameters["AttendedBy"];
            DateTime? paramRequestDateFrom = string.IsNullOrEmpty(parameters["ReqDateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateFrom"]);
            DateTime? paramRequestDateTo = string.IsNullOrEmpty(parameters["ReqDateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["ReqDateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<LegalClient>.Instance.GetLegal(appSettings.Value.ConnectionString, pageNumber, pageSize, paramType, paramUserID, paramStatus, paramUserName, paramSourceOU, paramSubject, paramRequestDateFrom, paramRequestDateTo, paramLabel, paramAttendedBy, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Legal/Home/Count/{UserID:int}")]
        public IActionResult GetHomeCount(int userID)
        {
            var response = DbClientFactory<LegalClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("Legal/Report")]
        public async Task<IActionResult> Export([FromBody]LegalReportRequestModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<LegalReportClient>.Instance.GetReportExporttList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Legal/Document")]
        public IActionResult SaveDocument([FromBody]DocumentPostModel report)
        {
            var result = DbClientFactory<LegalClient>.Instance.PostDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpPut]
        [Route("Legal/Document")]
        public IActionResult UpdateDocument([FromBody]DocumentPutModel report)
        {
            var result = DbClientFactory<LegalClient>.Instance.PutDocument(appSettings.Value.ConnectionString, report);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Legal/Document/{id:int}")]
        public IActionResult DeleteDocument(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var result = DbClientFactory<LegalClient>.Instance.DeleteDocument(appSettings.Value.ConnectionString, id, paramUserID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Legal/Document/{PageNumber:int},{PageSize:int}")]
        public IActionResult GetDocument(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = int.Parse(parameters["UserID"]);
            var paramType = parameters["Type"];
            var paramCreator = parameters["Creator"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<LegalClient>.Instance.GetDocument(appSettings.Value.ConnectionString, paramUserID, paramType, pageNumber, pageSize, paramCreator, paramSmartSearch, lang);
            return Ok(result);
        }
    }
}

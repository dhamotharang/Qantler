using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Calendar;
using RulersCourt.Repository.Calendar;
using RulersCourt.Translators.Calendar;
using System;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Calendar
{
    [EnableCors("AllowOrigin")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/")]
    public class CalendarController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;

        public CalendarController(IServiceProvider serviceProvider, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            workflow = serviceProvider.GetService<IWorkflowClient>();
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Calendar/{id:int}")]
        public IActionResult GetCalendarByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<CalendarClient>.Instance.GetCalendarByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Calendar")]
        public IActionResult SaveCalendar([FromBody]CalendarPostModel calendar)
        {
            var result = DbClientFactory<CalendarClient>.Instance.PostCalendar(appSettings.Value.ConnectionString, calendar);
            CalendarSaveResponseModel res = new CalendarSaveResponseModel();
            res.CalendarID = result.CalendarID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CalendarWorkflowTranslator().CalendarRequestGetWorkFlow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.CalendarID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPut]
        [Route("Calendar")]
        public IActionResult UpdateCalendar([FromBody]CalendarPutModel calendar)
        {
            var result = DbClientFactory<CalendarClient>.Instance.UpdateCalendar(appSettings.Value.ConnectionString, calendar);
            CalendarSaveResponseModel res = new CalendarSaveResponseModel();
            res.CalendarID = result.CalendarID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CalendarWorkflowTranslator().CalendarRequestGetWorkFlow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.CalendarID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpPatch]
        [Route("Calendar/{id}")]
        public IActionResult ModifyCalendar(int id, [FromBody]JsonPatchDocument<CalendarPutModel> calendar)
        {
            var result = DbClientFactory<CalendarClient>.Instance.PatchCalendar(appSettings.Value.ConnectionString, id, calendar);
            CalendarSaveResponseModel res = new CalendarSaveResponseModel();
            res.CalendarID = result.CalendarID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new CalendarWorkflowTranslator().CalendarRequestGetWorkFlow(result, appSettings.Value.ConnectionString);
            bo.ServiceID = result.CalendarID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(res);
        }

        [HttpGet]
        [Route("Calendar/ListView/{PageNumber:int},{PageSize:int}")]
        public IActionResult CalendarListView(int pageNumber, int pageSize)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramRefNo = parameters["ReferenceNumber"];
            var paramType = parameters["Type"];
            var paramEventType = parameters["EventType"];
            var paramEventRequestor = parameters["EventRequestor"];
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateTo"]);
            var paramStatus = parameters["Status"];
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<CalendarClient>.Instance.GetCalendarList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramRefNo, paramType, paramEventType, paramEventRequestor, paramDateFrom, paramDateTo, paramStatus, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Calendar/CalendarView")]
        public IActionResult CalenderView(int userID)
        {
            CalendarViewModel response = new CalendarViewModel();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var paramMonth = parameters["Month"];
            var paramYear = parameters["Year"];
            var paramRefNo = parameters["ReferenceNumber"];

            var paramEventRequestor = parameters["EventRequestor"];
            var paramEventType = parameters["EventType"];
            DateTime? paramDateFrom = string.IsNullOrEmpty(parameters["DateFrom"]) ? (DateTime?)null : DateTime.Parse(parameters["DateFrom"]);
            DateTime? paramDateTo = string.IsNullOrEmpty(parameters["DateTo"]) ? (DateTime?)null : DateTime.Parse(parameters["DateTo"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<CalendarClient>.Instance.GetCalenderViewList(appSettings.Value.ConnectionString, userID, paramMonth, paramYear, paramRefNo, paramEventRequestor, paramEventType, paramDateFrom, paramDateTo, lang, paramSmartSearch);
            return Ok(result);
        }

        [HttpGet]
        [Route("Calendar/ListBulkView")]
        public IActionResult CalendarListBulkView(int userID)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            CalendarBulkList response = new CalendarBulkList();

            var referenceNumber = Request.Query["ReferenceNumber"];
            var result = DbClientFactory<CalendarClient>.Instance.GetCalendarByBulkID(appSettings.Value.ConnectionString, userID, referenceNumber, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Calendar/Report")]
        public async Task<IActionResult> ExportAsync([FromBody]CalendarReportModel report, string type)
        {
            try
            {
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var paramSmartSearch = parameters["SmartSearch"];
                var result = DbClientFactory<CalendarClient>.Instance.GetCalendarReportExportList(appSettings.Value.ConnectionString, report, paramSmartSearch, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        [Route("Calendar/BulkAction")]
        public IActionResult BulkApproval([FromBody]CalendarBulkApprovalModel calendar)
        {
            int items = calendar.CalendarID.Count;
            for (int i = 0; i < items; i++)
            {
                int calendarID = calendar.CalendarID[i].CalendarID;
                var result = DbClientFactory<CalendarClient>.Instance.GetCalendarBulkWorkflowByID(appSettings.Value.ConnectionString, calendarID, calendar);
                Workflow.WorkflowBO bo = new CalendarWorkflowTranslator().CalendarRequestGetWorkFlow(result, appSettings.Value.ConnectionString);
                bo.ServiceID = calendarID;
                workflow.StartWorkflow(bo);
            }

            return Ok(1);
        }

        [HttpPost]
        [Route("Calendar/Apology")]
        public IActionResult Apology([FromBody]CalendarBulkApprovalModel calendar)
        {
            int items = calendar.CalendarID.Count;
            for (int i = 0; i < items; i++)
            {
                int calendarID = calendar.CalendarID[i].CalendarID;
                var result = DbClientFactory<CalendarClient>.Instance.BulkApology(appSettings.Value.ConnectionString, calendarID, calendar);
            }

            return Ok(true);
        }

        [HttpGet]
        [Route("Calendar/Count")]
        public IActionResult GetAllModulesPendingTasksCount()
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = parameters["UserID"] == null ? 0 : Convert.ToInt32(parameters["UserID"]);
            var response = DbClientFactory<CalendarClient>.Instance.GetAllModulesPendingTasksCount(appSettings.Value.ConnectionString, userID);
            return Ok(response);
        }

        [HttpPost]
        [Route("Calendar/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]CalendarHistoryPostModel value)
        {
            var result = DbClientFactory<CalendarClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            return Ok(result);
        }
    }
}

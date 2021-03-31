using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulersCourt.Models;
using RulersCourt.Models.Meeting;
using RulersCourt.Repository.Meeting;
using RulersCourt.Services;
using RulersCourt.Translators.Meeting;
using Shark.PdfConvert;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Workflow.Interface;

namespace RulersCourt.Controllers.Meeting
{
    [EnableCors("AllowOrigin")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/")]
    public class MeetingController : CommonController
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IWorkflowClient workflow;
        private readonly IHostingEnvironment environment;
        private readonly EncryptionService encryptionService;

        public MeetingController(IServiceProvider serviceProvider, EncryptionService encryptionSvc, IOptions<ConnectionSettingsModel> app, IHostingEnvironment env)
            : base(app, env)
        {
            appSettings = app;
            environment = env;
            workflow = serviceProvider.GetService<IWorkflowClient>();
            encryptionService = encryptionSvc;
        }

        [HttpGet]
        [Route("Meeting/ListView{PageNumber:int},{PageSize:int}")]
        public IActionResult MeetingListView(int pageNumber, int pageSize)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramUserID = parameters["UserID"];
            var paramRefNo = parameters["ReferenceNumber"];
            var paramSubject = parameters["Subject"];
            var paramLocation = parameters["Location"];
            var paramMeetingType = parameters["MeetingType"];
            DateTime? paramStartDate = string.IsNullOrEmpty(parameters["StartDateTime"]) ? (DateTime?)null : DateTime.Parse(parameters["StartDateTime"]);
            DateTime? paramEndDate = string.IsNullOrEmpty(parameters["EndDateTime"]) ? (DateTime?)null : DateTime.Parse(parameters["EndDateTime"]);
            var paramInvitees = parameters["Invitees"];
            var paramSmartSearch = parameters["SmartSearch"];
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MeetingClient>.Instance.GetMeetingList(appSettings.Value.ConnectionString, pageNumber, pageSize, paramUserID, paramRefNo, paramSubject, paramLocation, paramMeetingType, paramStartDate, paramEndDate, paramInvitees, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [Route("Meeting/CalendarView/{UserID:int}")]
        public IActionResult MeetingCalenderView(int userID)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var paramMonth = parameters["Month"];
            var paramYear = parameters["Year"];
            var paramRefNo = parameters["ReferenceNumber"];
            var paramSubject = parameters["Subject"];
            var paramLocation = parameters["Location"];
            var paramMeetingType = parameters["MeetingType"];
            DateTime? paramStartDate = string.IsNullOrEmpty(parameters["StartDateTime"]) ? (DateTime?)null : DateTime.Parse(parameters["StartDateTime"]);
            DateTime? paramEndDate = string.IsNullOrEmpty(parameters["EndDateTime"]) ? (DateTime?)null : DateTime.Parse(parameters["EndDateTime"]);
            var paramSmartSearch = parameters["SmartSearch"];
            var result = DbClientFactory<MeetingClient>.Instance.GetMeetingCalenderList(appSettings.Value.ConnectionString, userID, paramMonth, paramYear, paramRefNo, paramSubject, paramLocation, paramMeetingType, paramStartDate, paramEndDate, paramSmartSearch, lang);
            return Ok(result);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Meeting/{id:int}")]
        public IActionResult GetMeetingByID(int id)
        {
            var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
            int userID = int.Parse(Request.Query["UserID"]);
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MeetingClient>.Instance.GetMeetingByID(appSettings.Value.ConnectionString, id, userID, lang);
            return Ok(result);
        }

        [HttpPost]
        [Route("Meeting")]
        public IActionResult SaveMeeting([FromBody]MeetingPostModel meeting)
        {
            MeetingSaveResponseModel res = new MeetingSaveResponseModel();
            var result = DbClientFactory<MeetingClient>.Instance.PostMeeting(appSettings.Value.ConnectionString, meeting);
            res.MeetingID = result.MeetingID;
            res.ReferenceNumber = result.ReferenceNumber;
            res.Status = result.Status;
            Workflow.WorkflowBO bo = new MeetingWorkFlowTranslator().MeetingGetWorkFlow(result, appSettings.Value.ConnectionString, encryptionService, environment);
            bo.ServiceID = result.MeetingID ?? 0;
            workflow.StartWorkflow(bo);

            return Ok(res);
        }

        [HttpPut]
        [Route("Meeting")]
        public IActionResult UpdateMeeting([FromBody]MeetingPutModel meeting)
        {
            MeetingSaveResponseModel response = new MeetingSaveResponseModel();
            var result = DbClientFactory<MeetingClient>.Instance.PutMeeting(appSettings.Value.ConnectionString, meeting);
            response.MeetingID = result.MeetingID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new MeetingWorkFlowTranslator().MeetingGetWorkFlow(result, appSettings.Value.ConnectionString, encryptionService, environment);
            bo.ServiceID = result.MeetingID ?? 0;
            workflow.StartWorkflow(bo);

            return Ok(response);
        }

        [HttpPatch]
        [Route("Meeting/{id}")]
        public IActionResult ModifyMeeting(int id, [FromBody]JsonPatchDocument<MeetingPutModel> value)
        {
            MeetingSaveResponseModel response = new MeetingSaveResponseModel();
            var result = DbClientFactory<MeetingClient>.Instance.PatchMeeting(appSettings.Value.ConnectionString, id, value);
            response.MeetingID = result.MeetingID;
            response.ReferenceNumber = result.ReferenceNumber;
            response.Status = result.Status;
            Workflow.WorkflowBO bo = new MeetingWorkFlowTranslator().MeetingGetWorkFlow(result, appSettings.Value.ConnectionString, encryptionService, environment);
            bo.ServiceID = result.MeetingID ?? 0;
            workflow.StartWorkflow(bo);

            return Ok(response);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccessControlAttribute))]
        [Route("Meeting/MOM/{id:int}")]
        public IActionResult GetMOMByID(int id)
        {
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            int userID = int.Parse(Request.Query["UserID"]);
            var result = DbClientFactory<MeetingClient>.Instance.GetMOMByID(appSettings.Value.ConnectionString, id, userID, lang);

            return Ok(result);
        }

        [HttpPost]
        [Route("Meeting/MOM")]
        public IActionResult SaveMOM([FromBody]MOMPostModel meeting)
        {
            MOMSaveResponseModel response = new MOMSaveResponseModel();
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            var result = DbClientFactory<MeetingClient>.Instance.PostMOM(appSettings.Value.ConnectionString, meeting);
            response.MOMID = result.MOMID;
            var res = DbClientFactory<MeetingClient>.Instance.GetMeetingPreview(appSettings.Value.ConnectionString, meeting.MeetingID ?? 0, meeting.CreatedBy ?? 0, lang);
            MeetingPreviewToPdf(res);
            Workflow.WorkflowBO bo = new MeetingWorkFlowTranslator().MeetingGetWorkFlow(result, appSettings.Value.ConnectionString, encryptionService, environment);
            bo.ServiceID = result.MeetingID ?? 0;
            workflow.StartWorkflow(bo);

            return Ok(response);
        }

        [HttpPost]
        [Route("Meeting/CommunicationChat")]
        public IActionResult CommunicationChat([FromBody]MeetingCommunicationHistoryModel value)
        {
            var result = DbClientFactory<MeetingClient>.Instance.SaveCommunicationChat(appSettings.Value.ConnectionString, value);
            value.Action = "Add Comment";
            var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
            Workflow.WorkflowBO bo = new MeetingWorkFlowTranslator().GetDutyTaskCommunicationWorkflow(value, appSettings.Value.ConnectionString, lang);
            bo.ServiceID = value.MeetingID ?? 0;
            workflow.StartWorkflow(bo);
            return Ok(result);
        }

        [HttpPost]
        [Route("Meeting/MOM/Download/{id}")]
        public IActionResult DownloadPDF(int id)
        {
            try
            {
                int userID = int.Parse(Request.Query["UserID"]);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var result = DbClientFactory<MeetingClient>.Instance.GetMeetingPreview(appSettings.Value.ConnectionString, id, userID, lang);
                MeetingPreviewToPdf(result);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("Meeting/Report")]
        public async Task<IActionResult> Export([FromBody]MeetingReport report, string type)
        {
            try
            {
                var parameters = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var lang = HttpContext.Request.Headers["Language"].ToString().ToUpper();
                var paramSmartSearch = parameters["SmartSearch"];
                var result = DbClientFactory<MeetingClient>.Instance.GetMeetingReportExportList(appSettings.Value.ConnectionString, report, lang);
                return await this.Export(type, result);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        private void MeetingPreviewToPdf(MeetingPreviewModel result)
        {
            string appPath = Directory.GetCurrentDirectory();
            string downloadPath = appPath + @"\Downloads\";
            string finalPdf = downloadPath + result.ReferenceNumber + ".pdf";
            string htmlString = System.IO.File.ReadAllText(appPath + @"/Templates/MeetingPDFTemplate.html");

            ArabicConstantModel arabicValues = new ArabicConstantModel();
            htmlString = htmlString.Replace("{{MeetingTitle}}", arabicValues.GetMeetingReport);

            htmlString = htmlString.Replace("{{RefNoValue}}", result.ReferenceNumber);
            htmlString = htmlString.Replace("{{RefNoLabel}}", arabicValues.GetReferenceNumber);
            htmlString = htmlString.Replace("{{OrgDeptValue}}", result.OrganizerDepartmentID);
            htmlString = htmlString.Replace("{{OrgDeptLabel}}", arabicValues.GetMeetingOrganizerDepartment);

            htmlString = htmlString.Replace("{{OrgUserValue}}", result.OrganizerUserID);
            htmlString = htmlString.Replace("{{OrgUserLabel}}", arabicValues.GetMeetingNameOfOrganizer);
            htmlString = htmlString.Replace("{{LocValue}}", result.Location);
            htmlString = htmlString.Replace("{{LocLabel}}", arabicValues.GetMeetingYoursite);

            htmlString = htmlString.Replace("{{SubjectValue}}", result.Subject);
            htmlString = htmlString.Replace("{{SubjectLabel}}", arabicValues.GetSubject);

            htmlString = htmlString.Replace("{{AttendeeValue}}", result.Attendees);
            htmlString = htmlString.Replace("{{AttendeeLabel}}", arabicValues.GetMeetingAttendees);

            htmlString = htmlString.Replace("{{StartDtValue}}", DateTime.SpecifyKind(result.StartDateTime ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{StartDtLabel}}", arabicValues.GetMeetingStartingDate);
            htmlString = htmlString.Replace("{{StartTmValue}}", DateTime.SpecifyKind(result.StartDateTime ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime().ToString("hh:mm tt"));
            htmlString = htmlString.Replace("{{StartTmLabel}}", arabicValues.GetMeetingTime);

            htmlString = htmlString.Replace("{{EndDtValue}}", DateTime.SpecifyKind(result.EndDateTime ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy"));
            htmlString = htmlString.Replace("{{EndDtLabel}}", arabicValues.GetMeetingExpiryDate);
            htmlString = htmlString.Replace("{{EndTmValue}}", DateTime.SpecifyKind(result.EndDateTime ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime().ToString("hh:mm tt"));
            htmlString = htmlString.Replace("{{EndTmLabel}}", arabicValues.GetExpiryTime);

            htmlString = htmlString.Replace("{{PtsDiscussedValue}}", result.PointsDiscussed);
            htmlString = htmlString.Replace("{{PtsDiscussedLabel}}", arabicValues.GetMeetingPoints);

            htmlString = htmlString.Replace("{{PtsPendingValue}}", result.PendingPoints);
            htmlString = htmlString.Replace("{{PtsPendingLabel}}", arabicValues.GetStickingPoints);

            htmlString = htmlString.Replace("{{SuggestionValue}}", result.Suggestion);
            htmlString = htmlString.Replace("{{SuggestionLabel}}", arabicValues.GetMeetingSuggestion);

            PdfConvert.Convert(new PdfConversionSettings
            {
                Content = htmlString,
                OutputPath = finalPdf
            });
        }
    }
}

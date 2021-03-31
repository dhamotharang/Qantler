namespace RulersCourt
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Options;
    using RulersCourt.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Web;

    /// <summary>
    /// This class file contains access control related functionalities.
    /// </summary>
    public class AccessControlAttribute : IActionFilter
    {
        private readonly IOptions<ConnectionSettingsModel> appSettings;
        private readonly IAuthenticationService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessControlAttribute"/> class.
        /// </summary>
        /// <param name="app">app settings.</param>
        /// <param name="authSvc">authentication service.</param>
        public AccessControlAttribute(IOptions<ConnectionSettingsModel> app, IAuthenticationService authSvc)
        {
            this.appSettings = app;
            this.authService = authSvc;
        }

        /// <summary>
        /// This method executes when an action takes place.
        /// </summary>
        /// <param name="context">Action Executing Context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int serviceID = 0;
            try
            {
                serviceID = Convert.ToInt32(context.ActionArguments["id"]);
            }
            catch (Exception)
            {
                serviceID = 0;
            }

            string type = context.RouteData.Values["controller"].ToString();
            if (serviceID == 0 && type != "Contact" && type != "OfficialTask" && type != "Gift" && type != "Calendar")
                return;

            var conn = this.appSettings.Value.ConnectionString;
            Dictionary<string, string> dict = GetTypeAndTableName(type);
            string queryString = context.HttpContext.Request.QueryString.ToString();
            var parsed = HttpUtility.ParseQueryString(queryString);
            string method = context.HttpContext.Request.Method;
            var userID = parsed["UserID"];
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_ServiceID", serviceID),
                new SqlParameter("@P_Type", dict["Type"]),
                new SqlParameter("@P_ServiceIDName", dict["ServiceIDName"]),
                new SqlParameter("@P_TableName", dict["TableName"]),
                new SqlParameter("@P_Method", method)
            };

            string result = SqlHelper.ExecuteProcedureReturnString(conn, "AccessControl", param);
            if (result != "Success")
            {
                context.Result = new BadRequestObjectResult("Don't have access");
                return;
            }
        }

        /// <summary>
        /// This method executes when an action completes the process.
        /// </summary>
        /// <param name="context">context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // throw new NotImplementedException();
        }

        private static Dictionary<string, string> GetTypeAndTableName(string type)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            switch (type)
            {
                case "Memo":
                    dict.Add("Type", "Memo"); dict.Add("TableName", "Memo"); dict.Add("ServiceIDName", "MemoID");
                    break;
                case "Circular":
                    dict.Add("Type", "Circular"); dict.Add("TableName", "Circular"); dict.Add("ServiceIDName", "CircularID");
                    break;
                case "DutyTask":
                    dict.Add("Type", "DutyTask"); dict.Add("TableName", "DutyTask"); dict.Add("ServiceIDName", "TaskID");
                    break;
                case "LetterOutbound":
                    dict.Add("Type", "Outbound Letter"); dict.Add("TableName", "LettersOutbound"); dict.Add("ServiceIDName", "LetterID");
                    break;
                case "LetterInbound":
                    dict.Add("Type", "Inbound Letter"); dict.Add("TableName", "LettersInbound"); dict.Add("ServiceIDName", "LetterID");
                    break;
                case "CitizenAffair":
                    dict.Add("Type", "Citizen Affair"); dict.Add("TableName", "CitizenAffair"); dict.Add("ServiceIDName", "CitizenAffairID");
                    break;
                case "CAComplaintSuggestions":
                    dict.Add("Type", "CAComplaintSuggestions"); dict.Add("TableName", "CAComplaintSuggestions"); dict.Add("ServiceIDName", "CAComplaintSuggestionsID");
                    break;
                case "Maintenance":
                    dict.Add("Type", "Maintenance"); dict.Add("TableName", "Maintenance"); dict.Add("ServiceIDName", "MaintenanceID");
                    break;
                case "ITSupport":
                    dict.Add("Type", "ITSupport"); dict.Add("TableName", "ITSupport"); dict.Add("ServiceIDName", "ITSupportID");
                    break;
                case "Legal":
                    dict.Add("Type", "Legal"); dict.Add("TableName", "Legal"); dict.Add("ServiceIDName", "LegalID");
                    break;
                case "Campaign":
                    dict.Add("Type", "MediaNewCampaignRequest"); dict.Add("TableName", "MediaNewCampaignRequest"); dict.Add("ServiceIDName", "CampaignID");
                    break;
                case "Design":
                    dict.Add("Type", "MediaDesignRequest"); dict.Add("TableName", "MediaDesignRequest"); dict.Add("ServiceIDName", "DesignID");
                    break;
                case "DiwanIdentity":
                    dict.Add("Type", "DiwanIdentity"); dict.Add("TableName", "DiwanIdentity"); dict.Add("ServiceIDName", "DiwanIdentityID");
                    break;
                case "Photo":
                    dict.Add("Type", "MediaPhotoRequest"); dict.Add("TableName", "MediaPhotoRequest"); dict.Add("ServiceIDName", "PhotoID");
                    break;
                case "Photographer":
                    dict.Add("Type", "MediaNewPhotoGrapherRequest"); dict.Add("TableName", "MediaNewPhotographerRequest"); dict.Add("ServiceIDName", "PhotoGrapherID");
                    break;
                case "PressRelease":
                    dict.Add("Type", "MediaNewPressReleaseRequest"); dict.Add("TableName", "MediaNewPressReleaseRequest"); dict.Add("ServiceIDName", "PressReleaseID");
                    break;
                case "Leave":
                    dict.Add("Type", "Leave"); dict.Add("TableName", "Leave"); dict.Add("ServiceIDName", "LeaveID");
                    break;
                case "Announcement":
                    dict.Add("Type", "Announcement"); dict.Add("TableName", "Announcement"); dict.Add("ServiceIDName", "AnnouncementID");
                    break;
                case "BabyAddition":
                    dict.Add("Type", "BabyAddition"); dict.Add("TableName", "BabyAddition"); dict.Add("ServiceIDName", "BabyAdditionID");
                    break;
                case "Certificate":
                    dict.Add("Type", "Certificate"); dict.Add("TableName", "Certificate"); dict.Add("ServiceIDName", "CertificateID");
                    break;
                case "CVBank":
                    dict.Add("Type", "CVBank"); dict.Add("TableName", "CVBank"); dict.Add("ServiceIDName", "CVBankId");
                    break;
                case "HRComplaintSuggestion":
                    dict.Add("Type", "HRComplaintSuggestions"); dict.Add("TableName", "HRComplaintSuggestions"); dict.Add("ServiceIDName", "HRComplaintSuggestionsID");
                    break;
                case "Training":
                    dict.Add("Type", "Training"); dict.Add("TableName", "Training"); dict.Add("ServiceIDName", "TrainingID");
                    break;
                case "UserProfile":
                    dict.Add("Type", "UserProfile"); dict.Add("TableName", "UserProfile"); dict.Add("ServiceIDName", "UserProfileId");
                    break;
                case "Compensation":
                    dict.Add("Type", "Compensation"); dict.Add("TableName", "Compensation"); dict.Add("ServiceIDName", "CompensationID");
                    break;
                case "OfficialTask":
                    dict.Add("Type", "OfficialTask"); dict.Add("TableName", "OfficialTask"); dict.Add("ServiceIDName", "OfficialTaskID");
                    break;
                case "Gift":
                    dict.Add("Type", "Gift"); dict.Add("TableName", "Gift"); dict.Add("ServiceIDName", "GiftID");
                    break;
                case "Meeting":
                    dict.Add("Type", "Meeting"); dict.Add("TableName", "Meeting"); dict.Add("ServiceIDName", "MeetingID");
                    break;
                case "Calendar":
                    dict.Add("Type", "Calendar"); dict.Add("TableName", "Calendar"); dict.Add("ServiceIDName", "CalendarID");
                    break;
                case "Contact":
                    dict.Add("Type", "Contact"); dict.Add("TableName", "Contact"); dict.Add("ServiceIDName", "ContactID");
                    break;
            }

            return dict;
        }
    }
}

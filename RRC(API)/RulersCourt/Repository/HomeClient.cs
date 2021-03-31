using RulersCourt.Models;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Utility;

namespace RulersCourt.Repository
{
    public class HomeClient
    {
        public HomeModel GetModulesCount(string connString, string userID, string lang)
        {
            HomeModel list = new HomeModel();

            SqlParameter[] parama = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Language", lang),
            };

            list = SqlHelper.ExecuteProcedureReturnData<HomeModel>(connString, "Get_HomeModulesCount", r => r.TranslateAsDashboardList(), parama);

            return list;
        }

        public GlobalSearchListModel GetGlobalSearchList(string connString, string userID, string search, string lang, int type, int pageNumber, int pageSize)
        {
            GlobalSearchListModel globalSearchList = new GlobalSearchListModel();
            SqlParameter[] globalListParam = { new SqlParameter("@P_UserID", userID),
                                      new SqlParameter("@P_Search", search),
                                      new SqlParameter("@P_Language", lang),
                                      new SqlParameter("@P_PageNumber", pageNumber),
                                      new SqlParameter("@P_PageSize", pageSize),
                                      new SqlParameter("@P_Type", type)
            };
            globalSearchList.Collection = SqlHelper.ExecuteProcedureReturnData<List<GlobalSearchModel>>(connString, "GlobalSearch", r => r.TranslateAsGlobalSearchList(), globalListParam);

            globalSearchList.ModuleList = SqlHelper.ExecuteProcedureReturnData<List<M_ModuleModel>>(connString, "Get_M_Modules", r => r.TranslateAsModuleList());

            SqlParameter[] gModulecountParam = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_Search", search),
                new SqlParameter("@P_Language", lang),
                new SqlParameter("@P_Type", 0)
            };

            globalSearchList.ModulesCount = SqlHelper.ExecuteProcedureReturnData<GlobalSearchCountModel>(connString, "GlobalSearch", r => r.TranslateAsGlobalSearchCount(), gModulecountParam);

            globalSearchList.Count = globalSearchList.ModulesCount.Memos + globalSearchList.ModulesCount.Letters + globalSearchList.ModulesCount.DutyTask +
                                     globalSearchList.ModulesCount.Meetings + globalSearchList.ModulesCount.Circulars + globalSearchList.ModulesCount.Legal +
                                     globalSearchList.ModulesCount.Protocol + globalSearchList.ModulesCount.HR + globalSearchList.ModulesCount.CitizenAffair +
                                     globalSearchList.ModulesCount.Maintenance + globalSearchList.ModulesCount.IT;

            return globalSearchList;
        }

        public NotificationModel GetNotificationList(string connString, string userID, int pageNumber, int pageSize, NotificationConfigModel notificationConfig, string lang)
        {
            NotificationModel notification = new NotificationModel();
            SqlParameter[] notificationListParam = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_PageNumber", pageNumber),
                new SqlParameter("@P_PageSize", pageSize),
                new SqlParameter("@P_NotificationDays", notificationConfig.DeletionDay),
                new SqlParameter("@P_Method", 0),
                new SqlParameter("@P_Language", lang)
            };

            notification.Collection = SqlHelper.ExecuteProcedureReturnData<List<NotificationListModel>>(connString, "Get_NotificationList", r => r.TranslateAsNotificationList(), notificationListParam);

            SqlParameter[] notificationListCount = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_NotificationDays", notificationConfig.DeletionDay),
                new SqlParameter("@P_Method", 1),
                new SqlParameter("@P_Language", lang)
            };

            notification.Count = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnString(connString, "Get_NotificationList", notificationListCount));

            SqlParameter[] notificationCount = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_NotificationDays", notificationConfig.DeletionDay),
                new SqlParameter("@P_Method", 2),
                new SqlParameter("@P_Language", lang)
            };

            notification.NotificationCount = Convert.ToInt32(SqlHelper.ExecuteProcedureReturnString(connString, "Get_NotificationList", notificationCount));

            return notification;
        }

        public NotificationGetModel ReadNotification(string uiUrl, string connString, int? userID, int? iD, bool? isReadAll, string lang)
        {
            NotificationGetModel notification = new NotificationGetModel();
            SqlParameter[] notificationListParam = {
                new SqlParameter("@P_ID", iD),
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_ToReadAll", isReadAll),
                new SqlParameter("@P_Language", lang)
};

            notification = SqlHelper.ExecuteProcedureReturnData<NotificationGetModel>(connString, "Get_Notification", r => r.TranslateAsNotification(), notificationListParam);

            if (iD != 0)
            {
                notification.Content = GetMessage(notification, uiUrl);
                notification.Subject = GetSubject(notification);
            }

            return notification;
        }

        public string GetTemplate(string workflowProcess)
        {
            switch (workflowProcess)
            {
                case "ApprovalWorkflow":
                    return "Approve.html";
                case "RejectWorkflow":
                    return "Reject.html";
                case "EscalateWorkflow":
                    return "Escalate.html";
                case "RedirectWorkflow":
                    return "Redirect.html";
                case "ReturnWorkflow":
                    return "Return.html";
                case "ShareWorkflow":
                    return "Share.html";
                case "CloseWorkflow":
                    return "Close.html";
                case "DutyTaskCommunicationBoardWorkflow":
                    return "DutyTaskCommunicationChat.html";
                case "DutyTaskSubmissionWorkflow":
                    return "Submission.html";
                case "DutyTaskDeleteWorkflow":
                    return "Delete.html";
                case "DutyTaskCompleteWorkflow":
                    return "Complete.html";
                case "DutyTaskCloseWorkflow":
                    return "Close.html";
                case "AssignWorkflow":
                    return "Assign.html";
                case "ReopenWorkflow":
                    return "Reopen.html";
                case "MeetingInvitesWorkflow":
                    return "Invite.html";
                case "MeetingRescheduleWorkflow":
                    return "Reschedule.html";
                case "MeetingCancelWorkflow":
                    return "Cancel.html";
                case "MeetingMomCreatedWorkflow":
                    return "CreateMOM.html";
                case "MeetingChatWorkflow":
                    return "MeetingCommunicationChat.html";
                case "NotificationWorkflow":
                    return "TrainingRequestNotification.html";
                case "FineSubmissionWorkflow":
                    return "FineSubmission.html";
                case "VehicleRemainderWorkflow":
                    return "FineRemainder.html";
                case "VehicleReleaseConfirmWorkflow":
                    return "VehicleReleaseConfirm.html";
                case "VehicleReturnConfirmWorkflow":
                    return "VehicleReturnConfirm.html";
                case "VehicleReleaseWorkflow":
                    return "VehicleRelease.html";
                case "VehicleReturnWorkflow":
                    return "VehicleReturn.html";
                case "VehicleReleaseConfirmationRejectWorkflow":
                    return "Reject.html";
                case "TrainingNotificationWorkflow":
                    return "TrainingRequestNotification.html";
                case "TrainingNotificationToManagerWorkflow":
                    return "TrainingRequestNotificationToManager.html";
                case "VehicleCancelWorkflow":
                    return "VehicleCancel.html";
                default:
                    return "Submission.html";
            }
        }

        public string GetSubject(NotificationGetModel n)
        {
            ConstantModel model = new ConstantModel();
            string body = File.ReadAllText(Directory.GetCurrentDirectory() + "/MailTemplates/Subject/" + GetTemplate(n.Process));
            string serviceName = n.Service.Replace(" ", string.Empty);
            if (n.ReferenceNumber.Substring(n.ReferenceNumber.Length - 2) == "SC")
                serviceName = "SalaryCertificate";

            if (n.ReferenceNumber.Substring(n.ReferenceNumber.Length - 2) == "EC")
                serviceName = "ExperienceCertificate";

            body = body.Replace("{{ArService}}", n.Service == "Certificate" ? model.GetArabicService(serviceName) : model.GetArabicService(n.Service));
            body = body.Replace("{{ReferenceNumber}}", n.ReferenceNumber);
            return body;
        }

        public string GetMessage(NotificationGetModel n, string link)
        {
            string serviceName = n.Service.Replace(" ", string.Empty);
            ConstantModel model = new ConstantModel();
            string enFromName, arFromName;
            if (n.EnDelegateFromName is null)
            {
                enFromName = n.EnFromName;
                arFromName = n.ArFromName;
            }
            else
            {
                enFromName = n.EnDelegateFromName + " onbehalf of " + n.EnFromName;
                arFromName = n.ArDelegateFromName + " " + model.GetOnBehalfOf + " " + n.ArFromName;
            }

            if (n.ReferenceNumber.Substring(n.ReferenceNumber.Length - 2) == "SC")
                serviceName = "SalaryCertificate";

            if (n.ReferenceNumber.Substring(n.ReferenceNumber.Length - 2) == "EC")
                serviceName = "ExperienceCertificate";

            string body = File.ReadAllText(Directory.GetCurrentDirectory() + "/MailTemplates/Content/" + GetTemplate(n.Process));

            if (n.Process == "SubmissionWorkflow" && n.Service == "HRComplaintSuggestions" && n.IsAnonymous == true)
            {
                body = body.Replace("{{ArFromName}}", model.GetArabicService("Anonymous"));
                body = body.Replace("{{EnFromName}}", model.GetEnglishService("Anonymous"));
            }
            else
            {
                body = body.Replace("{{EnFromName}}", enFromName);
                body = body.Replace("{{ArFromName}}", arFromName);
            }

            string service = n.Service.Replace(" ", "_") == "Certificate" ? serviceName : n.Service.Replace(" ", "_");
            body = body.Replace("{{ToName}}", n.ToName);
            body = body.Replace("{{Service}}", service);
            body = body.Replace("{{ArService}}", n.Service == "Certificate" ? model.GetArabicService(serviceName) : model.GetArabicService(n.Service));
            body = body.Replace("{{EnService}}", n.Service == "Certificate" ? model.GetEnglishService(serviceName) : model.GetEnglishService(n.Service));
            body = body.Replace("{{Link}}", link);
            body = body.Replace("{{ServiceID}}", n.ServiceID.ToString());
            body = body.Replace("{{ReferenceNumber}}", n.ReferenceNumber);
            return body;
        }
    }
}

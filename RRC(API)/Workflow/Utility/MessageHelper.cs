using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Workflow.Utility
{
    public class MessageHelper
    {
        private Settings _settings;
        public MessageHelper(Settings settings)
        {
            _settings = settings;
        }

        public string GetOperation(WorkflowType workflowProcess)
        {
            switch (workflowProcess)
            {
                case WorkflowType.ApprovalWorkflow:
                    return "Approve.html";
                case WorkflowType.RejectWorkflow:
                    return "Reject.html";
                case WorkflowType.EscalateWorkflow:
                    return "Escalate.html";
                case WorkflowType.RedirectWorkflow:
                    return "Redirect.html";
                case WorkflowType.ReturnWorkflow:
                    return "Return.html";
                case WorkflowType.ShareWorkflow:
                    return "Share.html";
                case WorkflowType.CloseWorkflow:
                    return "Close.html";
                case WorkflowType.DutyTaskCommunicationBoardWorkflow:
                    return "DutyTaskCommunicationChat.html";
                case WorkflowType.DutyTaskSubmissionWorkflow:
                    return "Submission.html";
                case WorkflowType.DutyTaskDeleteWorkflow:
                    return "Delete.html";
                case WorkflowType.DutyTaskCompleteWorkflow:
                    return "Complete.html";
                case WorkflowType.DutyTaskCloseWorkflow:
                    return "Close.html";
                case WorkflowType.AssignWorkflow:
                    return "Assign.html";
                case WorkflowType.ReopenWorkflow:
                    return "Reopen.html";
                case WorkflowType.MeetingInvitesWorkflow:
                    return "Invite.html";
                case WorkflowType.MeetingRescheduleWorkflow:
                    return "Reschedule.html";
                case WorkflowType.MeetingCancelWorkflow:
                    return "Cancel.html";
                case WorkflowType.MeetingMomCreatedWorkflow:
                    return "CreateMOM.html";
                case WorkflowType.MeetingChatWorkflow:
                    return "MeetingCommunicationChat.html";
                case WorkflowType.NotificationWorkflow:
                    return "TrainingRequestNotification.html";
                case WorkflowType.FineSubmissionWorkflow:
                    return "FineSubmission.html";
                case WorkflowType.VehicleRemainderWorkflow:
                    return "FineRemainder.html";
                case WorkflowType.VehicleReleaseConfirmWorkflow:
                    return "VehicleReleaseConfirm.html";
                case WorkflowType.VehicleReturnConfirmWorkflow:
                    return "VehicleReturnConfirm.html";
                case WorkflowType.VehicleReleaseWorkflow:
                    return "VehicleRelease.html";
                case WorkflowType.VehicleReturnWorkflow:
                    return "VehicleReturn.html";
                case WorkflowType.VehicleReleaseConfirmationRejectWorkflow:
                    return "Reject.html";
				case WorkflowType.TrainingNotificationToManagerWorkflow:
					return "TrainingRequestNotificationToManager.html";
                case WorkflowType.VehicleCancelWorkflow:
                    return "VehicleCancel.html";

                default:
                    return "Submission.html";
            }
        }

        public String GetSubject(WorkflowBO BO)
        {
            ConstantModel arabic = new ConstantModel();
            string Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/MailTemplates/Subject/" + GetOperation(BO.WorkflowProcess));
            Body = Body.Replace("{{Service}}", BO.Service);

            Body = Body.Replace("{{ArService}}", arabic.GetArabicService(BO.Service));
            Body = Body.Replace("{{EnService}}", arabic.GetEnglishService(BO.Service));
            Body = Body.Replace("{{ReferenceNumber}}", BO.ReferenceNumber);
            Body = Body.Replace("\r\n", " ");
            return Body;
        }

        public String GetMessage(WorkflowBO BO, Actor to)
        {
            if (BO.WorkflowProcess == WorkflowType.NotificationWorkflow)
            {

            }
            var link = "";
            ConstantModel arabic = new ConstantModel();
            string ServiceName = BO.Service.Replace(" ", "");
            if (BO.ServiceID != 0)
            {
                link = _settings.UIConfigUrl;
            }
            String EnFromName, ArFromName;
            if (BO.DelegateFrom is null)
            {
                EnFromName = BO.From.Name; ArFromName = BO.From.ARName;
            }
            else
            {
                EnFromName = (BO.DelegateFrom.Name + " onbehalf of " + BO.From.Name);
                ArFromName = (BO.DelegateFrom.ARName + " " + arabic.GetOnBehalfOf + " " + BO.From.ARName);
            }

            if (BO.ReferenceNumber.Substring(BO.ReferenceNumber.Length - 2) == "SC")
                BO.Service = "SalaryCertificate";

            if (BO.ReferenceNumber.Substring(BO.ReferenceNumber.Length - 2) == "EC")
                BO.Service = "ExperienceCertificate";

            string Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/MailTemplates/Content/" + GetOperation(BO.WorkflowProcess));

			if (BO.WorkflowProcess.ToString() == "SubmissionWorkflow" && BO.Service == "HRComplaintSuggestions" && BO.IsAnonymous == true)
			{
				Body = Body.Replace("{{ArFromName}}", arabic.GetArabicService("Anonymous"));
				Body = Body.Replace("{{EnFromName}}", arabic.GetEnglishService("Anonymous"));
			}
			else
			{
				Body = Body.Replace("{{EnFromName}}", EnFromName);
				Body = Body.Replace("{{ArFromName}}", ArFromName);
			}

            string Service = BO.Service.Replace(" ", "_");
            Body = Body.Replace("{{ToName}}", to.Name);
            Body = Body.Replace("{{Service}}", Service);
            Body = Body.Replace("{{ArService}}", arabic.GetArabicService(BO.Service));
            Body = Body.Replace("{{EnService}}", arabic.GetEnglishService(BO.Service));
            Body = Body.Replace("{{Link}}", link);
            Body = Body.Replace("{{ServiceID}}", BO.ServiceID.ToString());
            Body = Body.Replace("{{ReferenceNumber}}", BO.ReferenceNumber);
            return Body;
            //return string.Format("Hi {0}, {1} <a href='{2}{6}/{3}'>#{4}</a> has been {5} by {7}", to.Name, BO.Service, link, BO.ServiceID, BO.ReferenceNumber, GetOperation(BO.WorkflowProcess), ServiceName, FromName);
        }

        public String GetSMSMessage(WorkflowBO BO, Actor to)
        {
            if (BO.WorkflowProcess == WorkflowType.NotificationWorkflow)
            {

            }
            var link = "";
            ConstantModel arabic = new ConstantModel();
            string ServiceName = BO.Service.Replace(" ", "");
            if (BO.ServiceID != 0)
            {
                link = _settings.UIConfigUrl;
            }
            String EnFromName, ArFromName;
            if (BO.DelegateFrom is null)
            {
                EnFromName = BO.From.Name; ArFromName = BO.From.ARName;
            }
            else
            {
                EnFromName = (BO.DelegateFrom.Name + " onbehalf of " + BO.From.Name);
                ArFromName = (BO.DelegateFrom.ARName + " " + arabic.GetOnBehalfOf + " " + BO.From.ARName);
            }

            if (BO.ReferenceNumber.Substring(BO.ReferenceNumber.Length - 2) == "SC")
                BO.Service = "SalaryCertificate";

            if (BO.ReferenceNumber.Substring(BO.ReferenceNumber.Length - 2) == "EC")
                BO.Service = "ExperienceCertificate";

            string Body = File.ReadAllText(Directory.GetCurrentDirectory() + "/SMSTemplates/Content/" + GetOperation(BO.WorkflowProcess).Replace(".html", ".txt"));

            if (BO.WorkflowProcess.ToString() == "SubmissionWorkflow" && BO.Service == "HRComplaintSuggestions" && BO.IsAnonymous == true)
            {
                Body = Body.Replace("{{ArFromName}}", arabic.GetArabicService("Anonymous"));
                Body = Body.Replace("{{EnFromName}}", arabic.GetEnglishService("Anonymous"));
            }
            else
            {
                Body = Body.Replace("{{EnFromName}}", EnFromName);
                Body = Body.Replace("{{ArFromName}}", ArFromName);
            }

            string Service = BO.Service.Replace(" ", "_");
            Body = Body.Replace("{{ToName}}", to.Name);
            Body = Body.Replace("{{Service}}", Service);
            Body = Body.Replace("{{ArService}}", arabic.GetArabicService(BO.Service));
            Body = Body.Replace("{{EnService}}", arabic.GetEnglishService(BO.Service));
            Body = Body.Replace("{{Link}}", link);
            Body = Body.Replace("{{ServiceID}}", BO.ServiceID.ToString());
            Body = Body.Replace("{{ReferenceNumber}}", BO.ReferenceNumber);
            var LRM = ((char)0x200E).ToString();
            Body = Body.Replace("{{ArReferenceNumber}}", LRM+BO.ReferenceNumber);

            return Body;
            //return string.Format("Hi {0}, {1} <a href='{2}{6}/{3}'>#{4}</a> has been {5} by {7}", to.Name, BO.Service, link, BO.ServiceID, BO.ReferenceNumber, GetOperation(BO.WorkflowProcess), ServiceName, FromName);
        }
    }
}
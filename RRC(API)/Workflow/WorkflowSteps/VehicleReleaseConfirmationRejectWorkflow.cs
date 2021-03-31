using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class VehicleReleaseConfirmationRejectWorkflow : IWorkflow
    {
        public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
        {
            try
            {
                if (wfObject.ServiceRequestor == null)
                    throw new Exception("Requestor name/email required for workflow");

                if (wfObject.From == null)
                    throw new Exception("From name/email required for workflow");

				foreach (var approver in wfObject.To)
				{
					//Put an entry into workflow table
					SqlParameter[] parameters = {
					new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
					new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
					new SqlParameter ("@P_Service", wfObject.Service),
					new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
					new SqlParameter("@P_FromName", wfObject.From.Name),
					new SqlParameter("@P_FromEmail", wfObject.From.Email),
					new SqlParameter("@P_ToName", approver.Name),
					new SqlParameter("@P_ToEmail", approver.Email),
					new SqlParameter("@P_Status", wfObject.Status),
					new SqlParameter("@P_DelegateFromEmail", wfObject.DelegateFrom is null? null : wfObject.DelegateFrom.Email),
					new SqlParameter("@P_DelegateFromName", wfObject.DelegateFrom  is null? null : wfObject.DelegateFrom.Name),
					new SqlParameter("@P_DelegateToEmail", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Email),
					new SqlParameter("@P_DelegateToName", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Name)};

					SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);

					SaveNotification.Save_Notification(settings, wfObject, approver);

					if (wfObject.ServiceRequestor.CanSendEmail && !string.IsNullOrEmpty(wfObject.ServiceRequestor.Email))
					{
						try
						{
							MailHelper helper = new MailHelper(settings);
							helper.SendEmail(wfObject, approver, true);
						}
						catch (Exception e)
						{
							new IssueLogger(settings.SysLogHost, settings.SysLogPort).LogIssue("Error:" + e.Message + "; Approver Name:" + wfObject.From.Name + "; Email:" + wfObject.From.Email);
						}
					}
					if (wfObject.SMSOrgDepUsers == null && wfObject.ServiceRequestor.CanSendSMS && !string.IsNullOrEmpty(wfObject.ServiceRequestor.PhoneNumber))
					{
						SMSHelper SMSHelper = new SMSHelper(settings);
						SMSHelper.SendSMS(wfObject, wfObject.ServiceRequestor);
					}
				}

				foreach (var SMSReceiver in wfObject.SMSOrgDepUsers)
				{
					if (SMSReceiver.CanSendSMS && !string.IsNullOrEmpty(SMSReceiver.PhoneNumber))
					{
						SMSHelper SMSHelper = new SMSHelper(settings);
						SMSHelper.SendSMS(wfObject, SMSReceiver);
					}
				}

				return "Workflow Complete";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class ApprovalWorkflow : IWorkflow
    {
        public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
        {
            try
            {
                if (wfObject.ServiceRequestor == null)
                    throw new System.Exception("Requestor name/email required for workflow");

                if (wfObject.From == null)
                    throw new System.Exception("From name/email required for workflow");

                if (wfObject.To == null || wfObject.To.Count <= 0)
                    throw new System.Exception("To name/email required for workflow");

                foreach (var receiver in wfObject.To)
                {
                    //Put an entry into workflow table
                    SqlParameter[] parameters = {
                    new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
                    new SqlParameter ("@P_Service", wfObject.Service),
					new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
					new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
                    new SqlParameter("@P_FromName", wfObject.From.Name),
                    new SqlParameter("@P_FromEmail", wfObject.From.Email),
                    new SqlParameter("@P_ToName", receiver.Name),
                    new SqlParameter("@P_ToEmail", receiver.Email),
                    new SqlParameter("@P_Status", wfObject.Status),
					new SqlParameter("@P_DelegateFromEmail", wfObject.DelegateFrom is null? null : wfObject.DelegateFrom.Email),
					new SqlParameter("@P_DelegateFromName", wfObject.DelegateFrom  is null? null : wfObject.DelegateFrom.Name),
					new SqlParameter("@P_DelegateToEmail", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Email),
					new SqlParameter("@P_DelegateToName", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Name)};

					SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);

					SaveNotification.Save_Notification(settings, wfObject, receiver);

					if (receiver.CanSendEmail && !string.IsNullOrEmpty(receiver.Email))
					{
						try
						{
							MailHelper helper = new MailHelper(settings);
							helper.SendEmail(wfObject, receiver,true);
						}
						catch (Exception e)
						{
							new IssueLogger(settings.SysLogHost, settings.SysLogPort).LogIssue("Error:" + e.Message + "; Approver Name:" + receiver.Name + "; Email:" + receiver.Email);
						}
					}

					if (wfObject.SMSOrgDepUsers == null && receiver.CanSendSMS && !string.IsNullOrEmpty(receiver.PhoneNumber))
					{
						SMSHelper SMSHelper = new SMSHelper(settings);
						SMSHelper.SendSMS(wfObject, receiver);
					}
				}

                foreach(var SMSReceiver in wfObject.SMSOrgDepUsers)
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
                throw new Exception("Workflow Incomplete");
            }
        }
    }
}

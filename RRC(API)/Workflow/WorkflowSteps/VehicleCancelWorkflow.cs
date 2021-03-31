using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Web;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
	class VehicleCancelWorkflow : IWorkflow
	{

		public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
		{
			try
			{
				if (wfObject.From == null)
					throw new System.Exception("From name/email required for workflow");

				if (wfObject.To == null || wfObject.To.Count <= 0)
					throw new System.Exception("To name/email required for workflow");

				MailHelper helper = new MailHelper(settings);
				foreach (var sharedrecipient in wfObject.To)
				{
					SaveNotification.Save_Notification(settings, wfObject, sharedrecipient);
					//Put an entry into workflow table
					SqlParameter[] parameters = {
					new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
					new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
					new SqlParameter ("@P_Service", wfObject.Service),
					new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
					new SqlParameter("@P_FromName", wfObject.From.Name),
					new SqlParameter("@P_FromEmail", wfObject.From.Email),
					new SqlParameter("@P_ToName", sharedrecipient.Name),
					new SqlParameter("@P_ToEmail", sharedrecipient.Email),
					new SqlParameter("@P_Status", wfObject.Status),
					new SqlParameter("@P_DelegateFromEmail", wfObject.DelegateFrom is null? null : wfObject.DelegateFrom.Email),
					new SqlParameter("@P_DelegateFromName", wfObject.DelegateFrom  is null? null : wfObject.DelegateFrom.Name),
					new SqlParameter("@P_DelegateToEmail", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Email),
					new SqlParameter("@P_DelegateToName", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Name)};


					SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);

					if (sharedrecipient.CanSendEmail && !string.IsNullOrEmpty(sharedrecipient.Email))
					{
						try
						{
							helper.SendEmail(wfObject, sharedrecipient,false);
						}
						catch (Exception e)
						{
							new IssueLogger(settings.SysLogHost, settings.SysLogPort).LogIssue("Error:" + e.Message + "; Approver Name:" + sharedrecipient.Name + "; Email:" + sharedrecipient.Email);
						}
					}
					if (sharedrecipient.CanSendSMS && !string.IsNullOrEmpty(sharedrecipient.PhoneNumber))
					{
						SMSHelper SMSHelper = new SMSHelper(settings);
						SMSHelper.SendSMS(wfObject, sharedrecipient);
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

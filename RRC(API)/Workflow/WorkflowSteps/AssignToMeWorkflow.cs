using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
	class AssignToMeWorkflow : IWorkflow
	{
		public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
		{
			try
			{
				foreach (var receiver in wfObject.To)
				{
					//Put an entry into workflow table
					SqlParameter[] parameters = {
					new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
					new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
					new SqlParameter ("@P_Service", wfObject.Service),
					new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
					new SqlParameter("@P_FromName", wfObject.From.Name),
					new SqlParameter("@P_FromEmail", wfObject.From.Email),
					new SqlParameter("@P_ToName", receiver.Name),
					new SqlParameter("@P_ToEmail", receiver.Email),
					new SqlParameter("@P_Status", wfObject.Status)};
					SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);

					SaveNotification.Save_Notification(settings, wfObject, receiver);

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

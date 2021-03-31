using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Workflow.Utility;

namespace Workflow
{
	public static class SaveNotification
	{
		public static void Save_Notification(Settings settings, WorkflowBO wfObject, Actor receiver)
		{
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
					new SqlParameter("@P_DelegateToName", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Name),
			    	new SqlParameter("@P_IsAnonymous", wfObject.IsAnonymous)};

			SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Notification", parameters);

		}
	}
}

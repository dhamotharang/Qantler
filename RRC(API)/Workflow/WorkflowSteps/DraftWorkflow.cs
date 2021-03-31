using System;
using System.Data.SqlClient;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class DraftWorkflow : IWorkflow
    {
        public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
        {
            if (wfObject.ServiceRequestor == null)
                throw new Exception("Requestor name/email required for workflow");
            //if (wfObject.To == null)
            //    throw new Exception("Approver name/email required for workflow");
            var name = ""; var Email = "";
			if (wfObject.To != null)
			{
				foreach (var to in wfObject.To)
				{
					name = to.Name;
					Email = to.Email;
				}
			}
            //Put an entry into workflow table
            SqlParameter[] parameters = {
                new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
				new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
				new SqlParameter ("@P_Service", wfObject.Service),
                new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
                new SqlParameter("@P_FromName", wfObject.ServiceRequestor.Name),
                new SqlParameter("@P_FromEmail", wfObject.ServiceRequestor.Email),
                new SqlParameter("@P_ToName", name),
                new SqlParameter("@P_ToEmail",Email),
                new SqlParameter("@P_Status", wfObject.Status)};

            return SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class SubmissionWorkflow : IWorkflow
    {
        private const int SyslogFacility = 16;

        public string ExecuteWorkflow(Settings settings, WorkflowBO wfObject)
        {

            try
            {
                if (wfObject.ServiceRequestor == null)
                    throw new System.Exception("Requestor name/email required for workflow");

                if (wfObject.To == null || wfObject.To.Count <= 0)
                    throw new System.Exception("To name/email required for workflow");

                foreach (var approver in wfObject.To)
                {
                    //Put an entry into workflow table
                    SqlParameter[] parameters = {
                    new SqlParameter ("@P_ReferenceNumber", wfObject.ReferenceNumber),
                    new SqlParameter ("@P_ServiceID", wfObject.ServiceID),
                    new SqlParameter ("@P_Service", wfObject.Service),
                    new SqlParameter("@P_WorkflowProcess", wfObject.WorkflowProcess.ToString()),
                    new SqlParameter("@P_FromName", wfObject.ServiceRequestor.Name),
                    new SqlParameter("@P_FromEmail", wfObject.ServiceRequestor.Email),
                    new SqlParameter("@P_ToName", approver.Name),
                    new SqlParameter("@P_ToEmail", approver.Email),
                    new SqlParameter("@P_Status", wfObject.Status),
                    new SqlParameter("@P_DelegateFromEmail", wfObject.DelegateFrom is null? null : wfObject.DelegateFrom.Email),
                    new SqlParameter("@P_DelegateFromName", wfObject.DelegateFrom  is null? null : wfObject.DelegateFrom.Name),
                    new SqlParameter("@P_DelegateToEmail", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Email),
                    new SqlParameter("@P_DelegateToName", wfObject.DelegateTo is null? null : wfObject.DelegateTo.Name)};


                    SQLHelper.ExecuteProcedureReturnString(settings.ConnectionString, "Save_Workflow", parameters);

                    SaveNotification.Save_Notification(settings, wfObject, approver);

                    if (approver.CanSendEmail && !string.IsNullOrEmpty(approver.Email))
                    {
                        try
                        {
                            MailHelper helper = new MailHelper(settings);
                            helper.SendEmail(wfObject, approver, false);
                        }
                        catch (Exception e)
                        {
                            new IssueLogger(settings.SysLogHost, settings.SysLogPort).LogIssue("Error:" + e.Message + "; Approver Name:" + approver.Name + "; Email:" + approver.Email);
                        }
                    }
                    if (approver.CanSendSMS && !string.IsNullOrEmpty(approver.PhoneNumber))
                    {
                        SMSHelper SMSHelper = new SMSHelper(settings);
                        SMSHelper.SendSMS(wfObject, approver);
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
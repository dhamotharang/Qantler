using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class VehicleRemainderWorkflow : IWorkflow
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

					SaveNotification.Save_Notification(settings, wfObject, approver);

					if (approver.CanSendEmail && !string.IsNullOrEmpty(approver.Email))
					{
						try
						{
							MailHelper helper = new MailHelper(settings);
							helper.SendEmail(wfObject, wfObject.To[0], true);
						}
						catch (Exception e)
						{
							new IssueLogger(settings.SysLogHost, settings.SysLogPort).LogIssue("Error:" + e.Message + "; Approver Name:" + wfObject.ServiceRequestor.Name + "; Email:" + wfObject.ServiceRequestor.Email);
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

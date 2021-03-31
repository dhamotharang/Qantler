using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow.WorkflowSteps
{
    class DutyTaskCompleteWorkflow : IWorkflow
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

					if (sharedrecipient.CanSendEmail && !string.IsNullOrEmpty(sharedrecipient.Email))
					{
						try
						{
							helper.SendEmail(wfObject, sharedrecipient,true);
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

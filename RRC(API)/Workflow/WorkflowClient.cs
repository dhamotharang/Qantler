using System;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow
{
    public class WorkflowClient : IWorkflowClient
    {
        Settings _settings;
        public WorkflowClient(Settings settings)
        {
            _settings = settings;
        }

        public WorkflowClient(string connString, string emailHost, int emailPort, string emailId, string password,string smsproviderURL, String sysloghost, int syslogport)
        {
            _settings = new Settings() { ConnectionString = connString, EmailHost = emailHost, EmailPort = emailPort, EmailId = emailId, Password = password, SMSProviderURL=smsproviderURL , SysLogHost=sysloghost,SysLogPort=syslogport };
        }

        /// <summary>
        /// Starts the flow based on Workflow type and Workflow object parameter.
        /// </summary>
        /// <param name="wfObj"></param>
        /// <returns></returns>
        public string StartWorkflow(WorkflowBO wfObj)
        {
            try
            {
                Workflow workflow = new Workflow(wfObj.WorkflowProcess);
                workflow.ExecuteWorkflow(_settings, wfObj);
            }
            catch (Exception e)
            {
				new IssueLogger(_settings.SysLogHost, _settings.SysLogPort).LogIssue(e.Message);
                //log to syslog
            }
            finally
            {
                //Close connection
            }
            return "";
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Workflow.Utility
{
    public class MailHelper
    {
        Settings _settings;
        public MailHelper(Settings settings)
        {
            _settings = settings;
        }

        public void SendEmail(WorkflowBO BO, Actor to, bool isCCRequired = true)
        {

            //smpt settings
            SmtpClient client = new SmtpClient(_settings.EmailHost, _settings.EmailPort);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_settings.EmailId, _settings.Password);
            client.EnableSsl = _settings.EnableSsl;

            MailMessage mailMessage = new MailMessage();
            MessageHelper helper = new MessageHelper(_settings);

            //Add Attachment
            if (BO.Attachment != null)
            {
                foreach (Attachment File in BO.Attachment)
                {
                    mailMessage.Attachments.Add(File);
                }
            }

            mailMessage.From = new MailAddress(_settings.EmailId);
            mailMessage.To.Add(to.Email);
            if (isCCRequired)
            {
                //mailMessage.CC.Add(from.Email);
                if (BO.ServiceRequestor != null && !to.Email.Equals(BO.ServiceRequestor.Email))
                    mailMessage.CC.Add(BO.ServiceRequestor.Email);
                if (BO.cc != null)
                {
                    foreach (Actor a in BO.cc)
                    {
                        if (!string.IsNullOrEmpty(a.Email))
                            mailMessage.CC.Add(a.Email);
                    }
                }
            }
            if (BO.DelegateTo != null)
                mailMessage.CC.Add(BO.DelegateTo.Email);

            if (BO.DelegateFrom != null)
                mailMessage.CC.Add(BO.From.Email);
            //string operation = MessageHelper.GetMessage(workflowProcess);


            mailMessage.Body = helper.GetMessage(BO, to);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = helper.GetSubject(BO);
            client.SendCompleted += new SendCompletedEventHandler(SendCompleted);
            client.SendAsync(mailMessage,"; Approver Name:" + to.Name + "; Email: " + to.Email);
        }
        public void SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if ( e.Error != null)
            {
                new IssueLogger(_settings.SysLogHost, _settings.SysLogPort).LogIssue("Error:" + e.Error.Message +  e.UserState.ToString());
            }
        }

    }
}
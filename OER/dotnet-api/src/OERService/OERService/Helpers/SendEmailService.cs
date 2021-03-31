using Core.Enums;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OERService.Helpers
{
	public class SendEmailService
    {
		readonly IConfiguration configuration;
        public SendEmailService()
        {
            configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
        }

        public string sendEmail(EmailHelperService obj)
        {
            var smtp = configuration.GetValue<string>("Smtp:Server");
            var port = configuration.GetValue<string>("Smtp:Port");
            var username = configuration.GetValue<string>("Smtp:emailusername");
            var password = configuration.GetValue<string>("Smtp:emailpassword");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtp);
            
            mail.From = new MailAddress(username);
            foreach (string to_address in obj.To)
            {
                mail.To.Add(to_address);
            }

            mail.Subject = obj.Subject;
            mail.Body = obj.MessageBody;
            mail.IsBodyHtml = true;
            SmtpServer.Port = Convert.ToInt32(port);
            SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }

            return "email send";
        }

        public async Task<string> SendEmailAsync(EmailHelperService obj)
        {
            
            var smtp = configuration.GetValue<string>("Smtp:Server");
            var port = configuration.GetValue<string>("Smtp:Port");
            var username = configuration.GetValue<string>("Smtp:emailusername");
            var password = configuration.GetValue<string>("Smtp:emailpassword");
            bool sslEnabled = configuration.GetValue<bool>("Smtp:ssl");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtp);

            mail.From = new MailAddress(username);
            foreach (string to_address in obj.To)
            {
                mail.To.Add(to_address);
            }

            mail.Subject = obj.Subject;
            mail.Body = obj.MessageBody;
            mail.IsBodyHtml = true;
            SmtpServer.Port = Convert.ToInt32(port);
            SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
            SmtpServer.EnableSsl = sslEnabled;
            try
            {
                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
			}

            return "email send";
        }

        public string SendEmailNotification(EmailHelperService obj)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpClient"]);

            mail.From = new MailAddress(ConfigurationManager.AppSettings["emailusername"]);
            foreach (string to_address in obj.To)
            {
                mail.To.Add(to_address);
            }

            mail.Subject = obj.Subject;
            mail.Body = obj.MessageBody;

            SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailusername"], ConfigurationManager.AppSettings["emailpassword"]);
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				throw;
            }

            return "email send";
        }
    }
}

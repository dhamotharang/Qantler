using MailReminder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace RrcRemainder
{
    class SendMailDutyTask
    {
        public MailMessage mail;
        public string fromAddress;
        public string smtpuser;
        public string smtppassword;
        public string smtphost;
		public int smtpPort;
		public bool smtpSsl;
		public string link;


		public SendMailDutyTask()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json");
            var configuration = builder.Build();
            EncryptionService service = new EncryptionService(Convert.FromBase64String(configuration["EncryptionKeys:Key"]), Convert.FromBase64String(configuration["EncryptionKeys:IV"]));
            fromAddress = service.Decrypt_Aes(configuration.GetSection("fromAddress").Value);
			smtphost = service.Decrypt_Aes(configuration.GetSection("smtphost").Value);
			smtpuser = service.Decrypt_Aes(configuration.GetSection("smtpuser").Value);
			smtppassword = service.Decrypt_Aes(configuration.GetSection("smtppassword").Value);
			smtpPort = Convert.ToInt32(service.Decrypt_Aes(configuration.GetSection("SMTPPort").Value));
			smtpSsl = Convert.ToBoolean(service.Decrypt_Aes(configuration.GetSection("EnableSsl").Value));
			link = service.Decrypt_Aes(configuration.GetSection("Url").Value);
		}

        public Boolean send(MailAddress addressess, DutyTask dt)
        {
            mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);
            using (SmtpClient smtp = new SmtpClient(smtphost, smtpPort))
            {
				smtp.EnableSsl = smtpSsl;
				smtp.UseDefaultCredentials = false; // [3] Changed this
				smtp.Credentials = new NetworkCredential(smtpuser, smtppassword);
				mail.IsBodyHtml = true;
				string url = link;
				url = url + "/DutyTask/" + dt.Id;
				//
				string Body = File.ReadAllText(Path.Combine("Templates", "DutyTaskTemplate.html"));
                Body.Replace("{{Id}}", dt.Id);
				//link = link+"/Task/" + dt.Id;
                Body = Body.Replace("{{ReferenceNumber}}", "<a href='" + url + "'>" + dt.ReferenceNumber.ToString() + "</a>");
				Body = Body.Replace("{{Title}}", dt.Title.ToString());
                Body = Body.Replace("{{Priority}}", dt.Priority.ToString());
                Body = Body.Replace("{{TaskDetails}}", dt.TaskDetails.ToString());
                Body = Body.Replace("{{StartDate}}", dt.StartDate.ToString());
                Body = Body.Replace("{{EndDate}}", dt.EndDate.ToString());
                mail.Subject = "RRC Remainder for DutyTask";
				//Console.WriteLine(Body);
                mail.Body = Body;
                mail.To.Add(addressess);

                try
                {
                    smtp.Send(mail);
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error(e, e.StackTrace);
                    return false;
                }
            }
        }
    }
}
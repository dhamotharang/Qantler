using MailReminder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace RrcRemainder
{
    class SendMailRrc
    {
        public string fromAddress;
        public string smtpuser;
        public string smtppassword;
        public string smtphost;
		public int smtpPort;
		public bool smtpSsl;
		public string link;

		public SendMailRrc()
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

        public MailMessage mail;
        public Boolean send(List<MailAddress> addressess, MeetingRrc meetingrrc)
        {
            mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);

            using (SmtpClient smtp = new SmtpClient(smtphost, smtpPort))
            {
                smtp.EnableSsl = smtpSsl;
                smtp.UseDefaultCredentials = false; // [3] Changed this
                smtp.Credentials = new NetworkCredential(smtpuser, smtppassword);
                mail.IsBodyHtml = true;
                //
                string Body = System.IO.File.ReadAllText(Path.Combine("Templates", "MeetingTemplate.html"));
                Body = Body.Replace("{{MeetingID}}", meetingrrc.MeetingID.ToString());
				link = link +"/Meeting/"+ meetingrrc.MeetingID;
				Body = Body.Replace("{{ReferenceNumber}}", "<a href='" + link + "'>" + meetingrrc.ReferenceNumber.ToString() + "</a>");
                Body = Body.Replace("{{Subject}}", meetingrrc.Subject.ToString());
                Body = Body.Replace("{{Location}}", meetingrrc.Location.ToString());
                Body = Body.Replace("{{StartDateTime}}", meetingrrc.StartDateTime.ToString());
                Body = Body.Replace("{{EndDateTime}}", meetingrrc.EndDateTime.ToString());
                mail.Body = Body;
                if (addressess.Count > 0)
                {
                    foreach (var maddress in addressess)
                    {
                        mail.To.Add(maddress);
                    }

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

                return false;
            }
        }
    }
}
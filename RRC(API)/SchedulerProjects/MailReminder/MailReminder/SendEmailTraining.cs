using Microsoft.Extensions.Configuration;
using RrcRemainder;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailReminder
{
	class SendEmailTraining
	{
		public string fromAddress;
		public string smtpuser;
		public string smtppassword;
		public string smtphost;
		public int smtpPort;
		public bool smtpSsl;
		public string link;

		public SendEmailTraining()
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
		public Boolean send(TrainingRrc Training)
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


				string Body = System.IO.File.ReadAllText(Path.Combine("Templates", "TrainingRequestNotificationContent.html"));
				Body = Body.Replace("{{ToName}}", Training.ToName);
				Body = Body.Replace("{{Link}}", link);
				Body = Body.Replace("{{Service}}", "Training");
				Body = Body.Replace("{{ServiceID}}", Training.TrainingID.ToString());
				Body = Body.Replace("{{ToName}}", Training.ToName);
				mail.Body = Body;


				string Subject = System.IO.File.ReadAllText(Path.Combine("Templates", "TrainingRequestNotificationSubject.html"));
				Subject = Subject.Replace("{{ArService}}", "طلبات التدريب");
				Subject = Subject.Replace("{{ReferenceNumber}}", Training.ReferenceNumber);
				mail.Subject = Subject;

				mail.To.Add(Training.ToEmail);

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


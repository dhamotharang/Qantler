using Core.Enums;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Helpers
{
	public class Emailer

	{
		internal DataAccessHelper _DataHelper = null;


		public Emailer(IConfiguration configuration)
		{

		}
		public static async Task<string> SendEmailAsync(UserEmail model, IConfiguration configuration)
		{
			bool isMailable = await Emailer.isUserEmailable(configuration, model.Email);
			if (isMailable)
			{
				SendEmailService _propService = new SendEmailService();
				EmailHelperService emailHelperService = new EmailHelperService();
				emailHelperService.To = new List<string>();
				emailHelperService.To.Add(model.Email);
				emailHelperService.Subject = model.Subject;
				emailHelperService.MessageBody = model.Body;
				await _propService.SendEmailAsync(emailHelperService);
				return "email sent";
			}
			else
			{
				return "User does not have emails enabled.";
			}
		}
		public static string CreateEmailBody(string UserName, string Url, string text, string buttonText,IConfiguration configuration)
		{
			string body = string.Empty;
			using (StreamReader reader =
			 new StreamReader(
				"./Helpers/EmailTemplates/email.html"))
			{
				body = reader.ReadToEnd();
			}

			body = body.Replace("{UserName}", UserName).
				Replace("{URL}", Url)
				.Replace("{Text1}", text)
				 .Replace(" {buttonText}", buttonText).Replace("{MailBannerURL}", configuration.GetSection("MailBannerURL").Value);

			return body;

		}

		public static string CreateEmailBody(string UserName, string Url, string text, string buttonText, int? portalLanguageId,IConfiguration configuration)
		{

			string body = string.Empty;
			if (portalLanguageId == 1)
			{
				using (StreamReader reader =
							 new StreamReader(
								"./Helpers/EmailTemplates/email_Ar.html"))
				{
					body = reader.ReadToEnd();
				}
			}
			else
			{
				using (StreamReader reader =
			 new StreamReader(
				"./Helpers/EmailTemplates/email.html"))
				{
					body = reader.ReadToEnd();
				}
			}


			body = body.Replace("{UserName}", UserName).
				Replace("{URL}", Url)
				.Replace("{Text1}", text)
				 .Replace(" {buttonText}", buttonText).Replace("{MailBannerURL}", configuration.GetSection("MailBannerURL").Value);

			return body;

		}

		public static string CreateEmailBodyForReply(string UserName, string Url, string text,IConfiguration configuration)
		{
			string body = string.Empty;
			using (StreamReader reader =
			 new StreamReader(
				"./Helpers/EmailTemplates/PostReply.html"))
			{
				body = reader.ReadToEnd();
			}

			body = body.Replace("{UserName}", UserName).
				Replace("{URL}", Url)
				.Replace("{Text1}", text).Replace("{MailBannerURL}", configuration.GetSection("MailBannerURL").Value);

			return body;

		}
		public static string CreateEmailBodyForTests(string UserName, string text,IConfiguration configuration)
		{
			string body = string.Empty;
			using (StreamReader reader =
			 new StreamReader(
				"./Helpers/EmailTemplates/email_Tests.html"))
			{
				body = reader.ReadToEnd();
			}

			body = body.Replace("{UserName}", UserName)
				.Replace("{Text}", text).Replace("{MailBannerURL}", configuration.GetSection("MailBannerURL").Value);

			return body;

		}


		public static async Task<bool> isUserEmailable(IConfiguration config, string email)
		{
			DataTable dt = new DataTable();
			SqlParameter[] parameters ={
					new SqlParameter( "@Email",  SqlDbType.NVarChar),
			};
			parameters[0].Value = email;
			DataAccessHelper _DataHelpers = new DataAccessHelper("sps_UserByEmail", parameters, config);
			await _DataHelpers.RunAsync(dt);
			try
			{
				if (dt != null && dt.Rows.Count > 0)
				{
					var user = (from model in dt.AsEnumerable()
								select new EmailNotification()
								{
									IsEmailNotification = model.Field<bool>("IsEmailNotification")

								}).FirstOrDefault();
					return user.IsEmailNotification;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Medium));
				return false;
				throw;
			}
			finally
			{
				_DataHelpers.Dispose();
			}
		}
	}
}

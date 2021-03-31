using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Workflow.Utility
{
	class SMSHelper
	{
		public static Settings _settings;
		public static Dictionary<string, string> dict = new Dictionary<string, string>();

		public SMSHelper(Settings settings)
		{
			_settings = settings;
			if(dict.Count==0)
			loadErrorList();
		}

		public static void loadErrorList()
		{
			dict.Add("10", "Sender id or title is blank");
			dict.Add("20", "Mobile number is blank");
			dict.Add("30", "Message text is blank");
			dict.Add("50", "Invalid mobile number (mobile number length is less that 8 digits)");
			dict.Add("200", "Message length exceeds 600 arabic chars");
			dict.Add("210", "Message length exceeds 1500 english chars");
			dict.Add("70", "Communication errork");
			dict.Add("80", "Communication error");
			dict.Add("81", "Communication error");
			dict.Add("110", "Communication error");
			dict.Add("90", "Invalid Main Account");
			dict.Add("91", "Invalid Sub Account");
			dict.Add("101", "Invalid password");
			dict.Add("150", "Main Account has no credit");
			dict.Add("151", "Sub Account has no credit");
			dict.Add("152", "Main Account is expired");
			dict.Add("Message can not be sent", "Error in processing the SMS");
		}
		

		public static void SendSMS(WorkflowBO BO, Actor To)
		{
            MessageHelper helper = new MessageHelper(_settings);
			using (HttpClient client = new HttpClient())
			{
				var builder = new UriBuilder(_settings.SMSProviderURL);
				var query = HttpUtility.ParseQueryString(builder.Query);
				query["urname"] = To.Name;
				query["number"] = To.PhoneNumber;
				byte[] ba = Encoding.BigEndianUnicode.GetBytes(helper.GetSMSMessage(BO, To));
				String strHex = BitConverter.ToString(ba);
				strHex = strHex.Replace("-", "");
				
			    query["udata"] = "|*UCS2|" + strHex;
				builder.Query = query.ToString();
				string url = builder.ToString();
				HttpResponseMessage response = client.GetAsync(url).Result;
				
				var result = response.Content.ReadAsStringAsync().Result;
				if (!response.IsSuccessStatusCode)
				{
					new IssueLogger(_settings.SysLogHost, _settings.SysLogPort).LogIssue("Error:"+response.ReasonPhrase + "; Name:" + To.Name + "; Phone:" + To.PhoneNumber);
				}
				else if  (dict.ContainsKey(result))
				{
					new IssueLogger(_settings.SysLogHost, _settings.SysLogPort).LogIssue(dict[response.Content.ReadAsStringAsync().Result] + "; Name:" + To.Name + "; Phone:" + To.PhoneNumber);
				}
			}
		}
	}
}

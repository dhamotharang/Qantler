using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Workflow.Interface;
using Workflow.Utility;

namespace Workflow
{
	class IssueLogger 
	{
		private const int SyslogFacility = 16;
		string sysloghost;
		int syslogport;

		public IssueLogger ( string sysloghost, int syslogport)
		{
			this.sysloghost = sysloghost;
			this.syslogport = syslogport;
		}

		public void LogIssue( string Message)
		{
			var hostName = Dns.GetHostName();
			var level = SyslogFacility * 8 + 4;
			var logMessage = string.Format("<{0}>{1} {2}", level, hostName, Message);
			var bytes = Encoding.UTF8.GetBytes(logMessage);
			using (var UDPclient = new UdpClient())
			{
				UDPclient.SendAsync(bytes, bytes.Length, sysloghost, syslogport).Wait();
			}
		}
	}
}

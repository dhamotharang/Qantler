using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RulersCourt.Models
{
    public class SyslogLogger : ILogger
    {
        private const int SyslogFacility = 16;
        private readonly Func<string, LogLevel, bool> _filter;
        private string _categoryName;
        private string _host;
        private int _port;

        public SyslogLogger(string categoryName, string host, int port, Func<string, LogLevel, bool> filter)
        {
            _categoryName = categoryName;
            _host = host;
            _port = port;

            _filter = filter;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{logLevel}: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception.ToString();
            }

            var syslogLevel = MapToSyslogLevel(logLevel);
            Send(syslogLevel, message);
        }

        internal void Send(SyslogLogLevel logLevel, string message)
        {
            if (string.IsNullOrWhiteSpace(_host) || _port <= 0)
            {
                return;
            }

            var hostName = Dns.GetHostName();
            var level = (SyslogFacility * 8) + (int)logLevel;
            message = message.Replace("\r\n", " ").Replace("\n", " ");
            var logMessage = string.Format("<{0}>{1} {2}", level, hostName, message);
            var bytes = Encoding.UTF8.GetBytes(logMessage);

            try
            {
                using (var tcp = new TcpClient())
                {
                    tcp.Connect(_host, _port);

                    using (var stream = tcp.GetStream())
                    {
                        stream.Write(bytes, 0, bytes.Length);
                        stream.WriteByte((byte)'\n');
                    }
                }
            }
            catch(Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("RRC-Syslog - " + ex.Message, EventLogEntryType.Error, 0, 0);
                }
            }
        }

        private SyslogLogLevel MapToSyslogLevel(LogLevel level)
        {
            if (level == LogLevel.Critical)
                return SyslogLogLevel.Critical;
            if (level == LogLevel.Debug)
                return SyslogLogLevel.Debug;
            if (level == LogLevel.Error)
                return SyslogLogLevel.Error;
            if (level == LogLevel.Information)
                return SyslogLogLevel.Info;
            if (level == LogLevel.None)
                return SyslogLogLevel.Info;
            if (level == LogLevel.Trace)
                return SyslogLogLevel.Info;
            if (level == LogLevel.Warning)
                return SyslogLogLevel.Warn;

            return SyslogLogLevel.Info;
        }
    }
}

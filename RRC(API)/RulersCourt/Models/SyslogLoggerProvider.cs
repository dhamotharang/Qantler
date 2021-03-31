using Microsoft.Extensions.Logging;
using System;

namespace RulersCourt.Models
{
    public class SyslogLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private string _host;
        private int _port;

        public SyslogLoggerProvider(string host, int port, Func<string, LogLevel, bool> filter)
        {
            _host = host;
            _port = port;

            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SyslogLogger(categoryName, _host, _port, _filter);
        }

        public void Dispose()
        {
        }
    }
}

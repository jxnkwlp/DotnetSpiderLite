using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    public class NullLogger : ILogger
    {
        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
        }
    }
}

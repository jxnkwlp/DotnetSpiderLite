using DotnetSpiderLite.Logs;
using System;
using System.Text;

namespace DotnetSpiderLite.Log4net
{
    public class Log4Logger : ILogger
    {
        private readonly log4net.ILog _log = null;

        public Log4Logger()
        {
            _log = log4net.LogManager.GetLogger(typeof(Spider));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Info:
                    return _log.IsInfoEnabled;
                case LogLevel.Warn:
                    return _log.IsWarnEnabled;
            }

            return false;
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(message, args);
            if (exception != null)
                sb.AppendLine(exception.ToString());

            switch (logLevel)
            {
                case LogLevel.Debug:
                    _log.Debug(string.Format(message, args), exception);
                    break;
                case LogLevel.Error:
                    _log.Error(string.Format(message, args), exception);
                    break;
                case LogLevel.Info:
                    _log.Info(string.Format(message, args), exception);
                    break;
                case LogLevel.Warn:
                    _log.Info(string.Format(message, args), exception);
                    break;
            }
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            Log(logLevel, null, message, args);
        }
    }
}

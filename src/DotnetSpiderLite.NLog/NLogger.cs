using DotnetSpiderLite.Logs;
using System;

namespace DotnetSpiderLite.NLogs
{
    public class NLogger : ILogger
    {
        private NLog.Logger _logger;

        public NLogger(string category)
        {
            _logger = NLog.LogManager.GetLogger(category);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(GetLogLevel(logLevel));
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            _logger.Log(GetLogLevel(logLevel), message, exception, args);
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            _logger.Log(GetLogLevel(logLevel), message, args);
        }

        private NLog.LogLevel GetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Info:
                    return NLog.LogLevel.Info;
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Warn:
                    return NLog.LogLevel.Warn;
                default:
                    return NLog.LogLevel.Off;
            }
        }
    }
}

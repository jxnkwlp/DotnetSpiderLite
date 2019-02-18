using DotnetSpiderLite.Logs;
using System;

namespace DotnetSpiderLite.Logs.DataBase
{
    public class Logger : ILogger
    {
        private ILoggerWriter _writer;
        private string _categoryName;

        public Logger(string categoryName, ILoggerWriter writer)
        {
            _categoryName = categoryName;
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            _writer.Write(_categoryName, logLevel, exception, message, args);
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            Log(logLevel, null, message, args);
        }

    }
}

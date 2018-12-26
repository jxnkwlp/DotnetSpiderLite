using DotnetSpiderLite.Logs;
using System;

namespace DotnetSpiderLite.Logs.DataBase
{
    public class LoggerFactory : ILoggerFactory
    {
        public ILoggerWriter LoggerWriter { get; set; }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (LoggerWriter == null)
                throw new ArgumentNullException(nameof(LoggerWriter));

            return new Logger(LoggerWriter);
        }

        public void Dispose()
        {
        }

    }
}

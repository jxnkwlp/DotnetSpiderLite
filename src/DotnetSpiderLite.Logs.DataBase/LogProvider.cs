using DotnetSpiderLite.Logs;
using System;
using System.Collections.Concurrent;

namespace DotnetSpiderLite.Logs.DataBase
{
    public class LogProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>();
        private readonly ILoggerWriter _loggerWriter;

        public LogProvider(ILoggerWriter LoggerWriter)
        {
            _loggerWriter = LoggerWriter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (_) =>
            {
                return new Logger(categoryName, _loggerWriter);
            });
        }

        public void Dispose()
        {
            _loggerWriter?.Dispose();
        }
    }
}

using DotnetSpiderLite.Logs;
using System;
using System.Collections.Concurrent;

namespace DotnetSpiderLite.NLogs
{
    public class NLogProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, NLogger> _loggers = new ConcurrentDictionary<string, NLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (_) =>
            {
                return new NLogger(categoryName);
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

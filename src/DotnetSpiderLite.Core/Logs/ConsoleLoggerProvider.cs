using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new ConcurrentDictionary<string, ConsoleLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, (_) =>
            {
                return new ConsoleLogger(categoryName);
            });
        }

        public void Dispose()
        {
        }
    }
}

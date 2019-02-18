using DotnetSpiderLite.Logs;
using System;

namespace DotnetSpiderLite.NLogs
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NLogLogger();
        }

        public void Dispose()
        {
        }

    }
}

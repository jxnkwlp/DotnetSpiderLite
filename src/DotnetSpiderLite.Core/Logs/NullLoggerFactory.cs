using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NullLogger();
        }

        public void Dispose()
        {
        }
    }
}

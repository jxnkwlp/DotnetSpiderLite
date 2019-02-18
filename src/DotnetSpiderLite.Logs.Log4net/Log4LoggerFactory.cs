using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Log4net
{
    public class Log4LoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Log4Logger();
        }

        public void Dispose()
        {
        }
    }
}

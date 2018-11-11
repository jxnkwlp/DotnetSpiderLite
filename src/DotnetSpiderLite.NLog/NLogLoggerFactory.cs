using DotnetSpiderLite.Logs;
using System;

namespace DotnetSpiderLite.NLogs
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(Type type)
        {
            return new NLogLogger();
        }
    }
}

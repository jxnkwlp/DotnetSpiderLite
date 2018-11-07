using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

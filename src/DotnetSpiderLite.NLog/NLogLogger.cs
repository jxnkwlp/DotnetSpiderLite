using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.NLogs
{
    public class NLogLogger : ILogger
    {
        NLog.Logger _logger = NLog.LogManager.GetLogger("spider");
         
        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}

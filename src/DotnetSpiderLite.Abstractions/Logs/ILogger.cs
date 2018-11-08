using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    /// <summary>
    ///  定义 logger 
    /// </summary>
    public interface ILogger
    {
        void Info(string message);
        void Error(string message, Exception ex = null);
        void Debug(string message);
        void Warn(string message);
        void Trace(string message);

    }
}

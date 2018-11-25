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
        bool IsEnabled(LogLevel logLevel);

        void Log(LogLevel logLevel, Exception exception, string message, params object[] args);

        void Log(LogLevel logLevel, string message, params object[] args);

        //bool IsEnabled { get; }
        //bool IsDebugEnabled { get; }
        //bool IsInfoEnabled { get; }
        //bool IsWarnEnabled { get; }
        //bool IsErrorEnabled { get; }

        //void Info(string message);
        //void Info(string message, params object[] args);
        //void Error(Exception ex);
        //void Error(string message, Exception ex = null, params object[] args););
        //void Debug(string message);
        //void Debug(string message, params object[] args);
        //void Warn(string message);
        //void Warn(string message, params object[] args);
        //void Trace(string message);
        //void Trace(string message, params object[] args);

    }
}

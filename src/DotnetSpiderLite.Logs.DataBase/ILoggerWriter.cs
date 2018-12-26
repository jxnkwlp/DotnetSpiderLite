using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs.DataBase
{
    /// <summary>
    ///  日志写数据库
    /// </summary>
    public interface ILoggerWriter
    {
        void Write(LogLevel logLevel, Exception exception, string message, params object[] args);
    }
}

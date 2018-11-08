using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    /// <summary>
    ///  ILoggerFactory
    /// </summary>
    public interface ILoggerFactory
    {
        ILogger GetLogger(Type type);
    }
}

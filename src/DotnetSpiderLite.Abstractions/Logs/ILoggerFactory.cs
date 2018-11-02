using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Abstractions.Logs
{
    /// <summary>
    ///  ILoggerFactory
    /// </summary>
    public interface ILoggerFactory
    {
        ILogger GetLogger(Type type);
    }
}

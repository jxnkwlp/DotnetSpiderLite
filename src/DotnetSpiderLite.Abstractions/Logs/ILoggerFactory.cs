using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    /// <summary>
    ///  获取 ILogger 
    /// </summary>
    public interface ILoggerFactory
    {
        ILogger GetLogger(Type type);
    }
}

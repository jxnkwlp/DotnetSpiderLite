using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    public interface ILoggerProvider : IDisposable
    {
        ILogger CreateLogger(string categoryName);
    }
}

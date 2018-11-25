using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Logs
{
    /// <summary> 
    ///  LogManager
    /// </summary>
    public static class LogManager
    {
        private static ILoggerFactory _defaultLoggerFactory = new ConsoleLoggerFactory();
        private static ILoggerFactory _loggerFactory;

        public static ILogger GetLogger(Type type)
        {
            var factory = _loggerFactory ?? _defaultLoggerFactory;
            return factory.CreateLogger(type.Name);
        }

        public static void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static void AddLoggerProvider(ILoggerProvider loggerProvider)
        {
            _loggerFactory.AddProvider(loggerProvider);
        }

    }
}

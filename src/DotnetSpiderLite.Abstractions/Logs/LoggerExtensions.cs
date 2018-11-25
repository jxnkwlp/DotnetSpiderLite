using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    /// <summary>
    ///  扩展
    /// </summary>
    public static class LoggerExtensions
    {
        public static void Info(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Info))
                logger.Log(LogLevel.Info, message, args);
        }

        public static void Info(this ILogger logger, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Info))
                logger.Log(LogLevel.Info, exception, message, args);
        }

        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.Log(LogLevel.Debug, message, args);
        }

        public static void Debug(this ILogger logger, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Debug))
                logger.Log(LogLevel.Debug, exception, message, args);
        }

        public static void Error(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
                logger.Log(LogLevel.Error, message, args);
        }

        public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Error))
                logger.Log(LogLevel.Error, exception, message, args);
        }

        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
                logger.Log(LogLevel.Trace, message, args);
        }

        public static void Trace(this ILogger logger, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Trace))
                logger.Log(LogLevel.Trace, exception, message, args);
        }

        public static void Warn(this ILogger logger, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warn))
                logger.Log(LogLevel.Warn, message, args);
        }

        public static void Warn(this ILogger logger, Exception exception, string message, params object[] args)
        {
            if (logger.IsEnabled(LogLevel.Warn))
                logger.Log(LogLevel.Warn, exception, message, args);
        }

    }
}

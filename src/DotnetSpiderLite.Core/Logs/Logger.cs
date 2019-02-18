using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    internal class Logger : ILogger
    {
        public LoggerInformation[] Loggers { get; set; }
        public MessageLogger[] MessageLoggers { get; set; }

        public bool IsEnabled(LogLevel logLevel)
        {
            var loggers = this.MessageLoggers;

            if (loggers == null || loggers.Length == 0)
                return false;

            foreach (var item in loggers)
            {
                if (item.IsEnabled(logLevel))
                    return true;
            }

            return false;
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            var loggers = this.MessageLoggers;

            if (loggers == null || loggers.Length == 0)
                return;

            List<Exception> exceptions = new List<Exception>();

            foreach (var logger in loggers)
            {
                if (!logger.IsEnabled(logLevel))
                {
                    continue;
                }

                try
                {
                    logger.Logger.Log(logLevel, exception, message, args);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("An error occurred while writing to logger(s).", exceptions);
            }

        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            Log(logLevel, null, message, args);
        }
    }

    internal struct MessageLogger
    {
        public MessageLogger(ILogger logger, string category, Type providerType, LogLevel? minLevel, Func<string, string, LogLevel, bool> filter)
        {
            Logger = logger;
            Category = category;
            ProviderType = providerType;
            MinLevel = minLevel;
            Filter = filter;
        }

        public ILogger Logger { get; }

        public string Category { get; }

        public Type ProviderType { get; }

        public LogLevel? MinLevel { get; }

        public Func<string, string, LogLevel, bool> Filter { get; }

        public bool IsEnabled(LogLevel level)
        {

            if (MinLevel != null && level < MinLevel)
            {
                return false;
            }

            if (Filter != null)
            {
                return Filter(ProviderType.FullName, Category, level);
            }

            return true;
        }
    }


    internal struct LoggerInformation
    {
        public LoggerInformation(ILoggerProvider provider, string category) : this()
        {
            ProviderType = provider.GetType();
            Logger = provider.CreateLogger(category);
            Category = category;
        }

        public ILogger Logger { get; }

        public string Category { get; }

        public Type ProviderType { get; }

        public bool ExternalScope { get; }
    }

}

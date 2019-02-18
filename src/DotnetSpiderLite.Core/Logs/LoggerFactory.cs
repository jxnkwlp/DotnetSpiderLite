using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Logs
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>(StringComparer.Ordinal);
        private IList<ILoggerProvider> _providers = new List<ILoggerProvider>();
        private static object _sync = new object();

        public void AddProvider(ILoggerProvider provider)
        {
            lock (_sync)
            {
                _providers.Add(provider);

                foreach (var existLogger in _loggers)
                {
                    var logger = existLogger.Value;
                    var loggerInformation = logger.Loggers;

                    var newLoggerIndex = loggerInformation.Length;
                    Array.Resize(ref loggerInformation, loggerInformation.Length + 1);
                    loggerInformation[newLoggerIndex] = new LoggerInformation(provider, existLogger.Key);

                    logger.MessageLoggers = CreateMessageLogger(loggerInformation);
                }
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            lock (_sync)
            {
                if (!_loggers.TryGetValue(categoryName, out var logger))
                {
                    logger = new Logger()
                    {
                        Loggers = CreateLoggers(categoryName),
                    };

                    logger.MessageLoggers = CreateMessageLogger(logger.Loggers);

                    _loggers[categoryName] = logger;
                }

                return logger;
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                foreach (var provider in _providers)
                {
                    try
                    {
                        provider.Dispose();
                    }
                    catch
                    {
                        // nothing todo 
                    }

                }
            }
        }


        private LoggerInformation[] CreateLoggers(string category)
        {
            var list = new LoggerInformation[_providers.Count];
            for (int i = 0; i < _providers.Count; i++)
            {
                list[i] = new LoggerInformation(_providers[i], category);
            }

            return list;
        }

        private MessageLogger[] CreateMessageLogger(LoggerInformation[] loggers)
        {
            var result = new MessageLogger[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
            {
                var log = loggers[i];
                result[i] = new MessageLogger(log.Logger, log.Category, log.ProviderType, null, (___, __, _) => true);
            }

            return result;
        }

    }
}

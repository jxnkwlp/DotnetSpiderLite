using DotnetSpiderLite.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Logs
{
    public class ConsoleLogger : ILogger
    {
        private string _name;

        public ConsoleLogger(string name)
        {
            _name = name;
        }

        //static Dictionary<string, ConsoleColor> LoggerColor = new Dictionary<string, ConsoleColor> {
        //    { "Debug", ConsoleColor.Black },
        //    { "Error", ConsoleColor.Red },
        //    { "Info", ConsoleColor.DarkGray },
        //    { "Trace", ConsoleColor.DarkYellow },
        //    { "Warn", ConsoleColor.Yellow }
        //};

        //private void Write(string level, string message)
        //{
        //    Console.ForegroundColor = LoggerColor[level];
        //    if (message.Contains("\n"))
        //    {
        //        Console.WriteLine($"[{DateTime.Now} {level}][{_type.Name}]");
        //        Console.WriteLine($"{message}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"[{DateTime.Now} {level}][{_type.Name}]{message}");
        //    }
        //    Console.ResetColor();
        //}

        //public void Debug(string message)
        //{
        //    Write("Debug", message);
        //}

        //public void Error(string message, Exception ex = null)
        //{
        //    Write("Error", message);
        //    Write("Error", ex.Message);
        //}

        //public void Info(string message)
        //{
        //    Write("Info", message);
        //}

        //public void Trace(string message)
        //{
        //    Write("Trace", message);
        //}

        //public void Warn(string message)
        //{
        //    Write("Warn", message);
        //}


        private readonly ConsoleColor? DefaultConsoleColor = null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            if (!IsEnabled(logLevel))
                return;

            if (exception != null || !string.IsNullOrEmpty(message))
            {
                WriteMessage(logLevel, _name, exception, message, args);
            }
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            WriteMessage(logLevel, _name, null, message, args);
        }

        private void WriteMessage(LogLevel logLevel, string logName, Exception exception, string message, params object[] args)
        {
            StringBuilder sb = new StringBuilder();

            var logLevelColors = GetLogLevelConsoleColors(logLevel);
            var logLevelString = GetLogLevelString(logLevel);

            sb.Append(DateTime.Now + " ");
            sb.Append(logLevelString + " ");
            sb.Append(logName + " ");

            if (!string.IsNullOrEmpty(message))
            {
                sb.AppendFormat(message, args);
            }
            if (exception != null)
            {
                // exception message
                sb.AppendLine(exception.ToString());
            }

            if (logLevelColors.Foreground.HasValue)
                Console.ForegroundColor = logLevelColors.Foreground.Value;
            if (logLevelColors.Background.HasValue)
                Console.BackgroundColor = logLevelColors.Background.Value;

            Console.WriteLine(sb.ToString());

            Console.ResetColor();

        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "trce";
                case LogLevel.Debug:
                    return "dbug";
                case LogLevel.Info:
                    return "info";
                case LogLevel.Warn:
                    return "warn";
                case LogLevel.Error:
                    return "fail";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    return new ConsoleColors(ConsoleColor.Black, ConsoleColor.Red);
                case LogLevel.Warn:
                    return new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black);
                case LogLevel.Info:
                    return new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black);
                case LogLevel.Debug:
                    return new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black);
                case LogLevel.Trace:
                    return new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black);
                default:
                    return new ConsoleColors(DefaultConsoleColor, DefaultConsoleColor);
            }
        }

        private struct ConsoleColors
        {
            public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
            {
                Foreground = foreground;
                Background = background;
            }

            public ConsoleColor? Foreground { get; }

            public ConsoleColor? Background { get; }
        }

    }
}

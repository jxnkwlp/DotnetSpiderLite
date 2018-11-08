using DotnetSpiderLite.Abstractions.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite.Logs
{
    public class ConsoleLogger : ILogger
    {
        Type _type;

        public ConsoleLogger(Type type)
        {
            _type = type;
        }

        static Dictionary<string, ConsoleColor> LoggerColor = new Dictionary<string, ConsoleColor> {
            { "Debug", ConsoleColor.Black },
            { "Error", ConsoleColor.Red },
            { "Info", ConsoleColor.DarkGray },
            { "Trace", ConsoleColor.DarkYellow },
            { "Warn", ConsoleColor.Yellow }
        };

        private void Write(string level, string message)
        {
            Console.ForegroundColor = LoggerColor[level];
            if (message.Contains("\n"))
            {
                Console.WriteLine($"[{_type.Name}]");
                Console.WriteLine($"{message}");
            }
            else
            {
                Console.WriteLine($"[{_type.Name}]{message}");
            }
            Console.ResetColor();
        }

        public void Debug(string message)
        {
            Write("Debug", message);
        }

        public void Error(string message, Exception ex = null)
        {
            Write("Error", message);
            Write("Error", ex.Message);
        }

        public void Info(string message)
        {
            Write("Info", message);
        }

        public void Trace(string message)
        {
            Write("Trace", message);
        }

        public void Warn(string message)
        {
            Write("Warn", message);
        }
    }
}

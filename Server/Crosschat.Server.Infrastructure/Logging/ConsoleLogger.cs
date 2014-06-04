using System;
using Crosschat.Utils.Logging;

namespace Crosschat.Server.Infrastructure.Logging
{
    /// <summary>
    /// TODO: replace with NLog
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private static readonly Object SyncObj = new object();

        private readonly string _typeName;

        public ConsoleLogger(string typeName)
        {
            _typeName = typeName;
        }

        public void Exception(Exception exc)
        {
            Write(ConsoleColor.DarkRed, GetExceptionDescription(exc));
        }

        public void Exception(Exception exc, string captionFormat, params object[] args)
        {
            Write(ConsoleColor.DarkRed, "{0}: {1}", string.Format(captionFormat, args), GetExceptionDescription(exc));
        }

        public void Error(string format, params object[] args)
        {
            Write(ConsoleColor.Red, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            Write(ConsoleColor.Yellow, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Write(ConsoleColor.White, format, args);
        }

        public void Debug(string format, params object[] args)
        {
            Write(ConsoleColor.Gray, format, args);
        }

        public void Trace(string format, params object[] args)
        {
            Write(ConsoleColor.DarkGray, format, args);
        }

        private string GetExceptionDescription(Exception exc)
        {
            int level = 0;
            string details = "";
            while (exc != null)
            {
                details += string.Format("{3}) Type: {0}. Message: {1}. StackTrace: {2}\n", exc.GetType().Name, exc.Message, exc.StackTrace, level);
                exc = exc.InnerException;
                level++;
            }
            return details;
        }

        private void Write(ConsoleColor color, string format, params object[] args)
        {
            try
            {
                lock (SyncObj)
                {
                    var prevColor = Console.ForegroundColor;
                    Console.ForegroundColor = color;
                    Console.WriteLine("{0} | {1} | {2}", DateTime.Now.ToString("HH:mm:ss"), _typeName, string.Format(format, args));
                    Console.ForegroundColor = prevColor;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("____________" + exc.Message);
            }
        }
    }
}

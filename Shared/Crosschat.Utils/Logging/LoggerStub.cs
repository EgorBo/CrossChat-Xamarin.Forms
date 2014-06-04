using System;

namespace Crosschat.Utils.Logging
{
    public class LoggerStub : ILogger
    {
        public void Exception(Exception exc)
        {
        }

        public void Exception(Exception exc, string captionFormat, params object[] args)
        {
        }

        public void Error(string format, params object[] args)
        {
        }

        public void Warning(string format, params object[] args)
        {
        }

        public void Info(string format, params object[] args)
        {
        }

        public void Debug(string format, params object[] args)
        {
        }

        public void Trace(string format, params object[] args)
        {
        }
    }
}

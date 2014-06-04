using System;

namespace Crosschat.Utils.Logging
{
    public interface ILogger
    {
        void Exception(Exception exc);

        void Exception(Exception exc, string captionFormat, params object[] args);

        void Error(string format, params object[] args);

        void Warning(string format, params object[] args);

        void Info(string format, params object[] args);

        void Debug(string format, params object[] args);

        void Trace(string format, params object[] args);
    }
}

using System;

namespace Crosschat
{
    public static class StringExtensions
    {
        public static string ToShortSizeInBytesString(long bytes)
        {
            const int kb = 1024;
            const int mb = kb*kb;
            const int gb = mb*kb;

            const string format = "{0} {1}";

            if (bytes < kb)

            if (bytes >= gb)
                return string.Format(format, Math.Round(bytes / (double)gb, 1), "Gb");

            if (bytes >= mb)
                return string.Format(format, Math.Round(bytes / (double)mb, 1), "Mb");

            if (bytes >= kb)
                return string.Format(format, Math.Round(bytes / (double)kb, 1), "Kb");

            return string.Format(format, bytes, "bytes");
        }

        public static string CutIfLonger(this string source, int maxLength = 1000)
        {
            if (String.IsNullOrEmpty(source))
                return source;
            if (source.Length < maxLength)
                return source;
            return source.Remove(maxLength - 1);
        }
    }
}

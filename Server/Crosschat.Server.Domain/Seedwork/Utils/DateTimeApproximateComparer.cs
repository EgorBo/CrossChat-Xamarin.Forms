using System;
using System.Collections.Generic;

namespace Crosschat.Server.Domain.Seedwork.Utils
{
    public class DateTimeApproximateComparer : IEqualityComparer<DateTime>
    {
        public bool Equals(DateTime x, DateTime y)
        {
            if (x == y)
                return true;
            if (Math.Abs((x - y).TotalSeconds) < 1)
                return true;
            return false;
        }

        public int GetHashCode(DateTime obj)
        {
            return obj.GetHashCode();
        }
    }
}
namespace Crosschat
{
    public static class PrimitiveTypeExtensions
    {
        public static bool ToBoolean(this object obj)
        {
            if (obj is bool)
                return (bool) obj;
            return false;
        }
    }
}
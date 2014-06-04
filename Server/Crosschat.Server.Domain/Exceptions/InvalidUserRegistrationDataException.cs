using System;

namespace Crosschat.Server.Domain.Exceptions
{
    public class InvalidUserRegistrationDataException : Exception
    {
        public InvalidUserRegistrationDataException(string message) : base(message)
        {
        }
    }
}

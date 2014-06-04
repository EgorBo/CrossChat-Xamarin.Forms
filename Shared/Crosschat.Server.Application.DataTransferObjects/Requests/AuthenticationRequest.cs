using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class AuthenticationRequest : RequestBase
    {
        public string Huid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationResponse : ResponseBase
    {
        public AuthenticationResponseType Result { get; set; }
        public UserDto User { get; set; }
    }
}
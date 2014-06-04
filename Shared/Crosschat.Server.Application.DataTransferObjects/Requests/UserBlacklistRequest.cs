using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class UserBlacklistRequest : RequestBase
    {
    }
    public class UserBlacklistResponse : ResponseBase
    {
        public UserDto[] Blacklist { get; set; }
    }
}
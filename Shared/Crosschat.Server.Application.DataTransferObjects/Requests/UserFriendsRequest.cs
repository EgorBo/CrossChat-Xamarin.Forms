using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class UserFriendsRequest : RequestBase
    {
    }
    public class UserFriendsResponse : ResponseBase
    {
        public UserDto[] Friends { get; set; }
    }
}
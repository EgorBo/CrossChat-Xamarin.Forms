using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class GetOnlineUsersRequest : RequestBase { }
    public class GetOnlineUsersResponse : ResponseBase
    {
        public UserDto[] Users { get; set; }
    }
}
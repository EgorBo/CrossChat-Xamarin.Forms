using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class UsersSearchRequest : RequestBase
    {
        public string QueryString { get; set; }
    }
    public class UsersSearchResponse : ResponseBase
    {
        public UserDto[] Result { get; set; }
    }
}

using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class GetUserDetailsRequest : RequestBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
    public class GetUserDetailsResponse : ResponseBase
    {
        public UserDto User { get; set; }
        public bool IsFriend { get; set; }
    }
}
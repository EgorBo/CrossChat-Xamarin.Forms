namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class RemoveFromFriendsRequest : RequestBase
    {
        public int TargetUserId { get; set; }
    }
    public class RemoveFromFriendsResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}
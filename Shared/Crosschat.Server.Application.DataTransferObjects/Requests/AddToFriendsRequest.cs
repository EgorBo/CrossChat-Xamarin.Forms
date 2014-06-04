namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class AddToFriendsRequest : RequestBase
    {
        public int TargetUserId { get; set; } 
    }
    public class AddToFriendsResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}
namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class RemoveFromBlacklistRequest : RequestBase
    {
        public int TargetUserId { get; set; }
    }
    public class RemoveFromBlacklistResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}
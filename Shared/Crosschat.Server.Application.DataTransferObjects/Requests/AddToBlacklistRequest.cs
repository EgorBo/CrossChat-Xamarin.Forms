namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class AddToBlacklistRequest : RequestBase
    {
        public int TargetUserId { get; set; }
    }
    public class AddToBlacklistResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}
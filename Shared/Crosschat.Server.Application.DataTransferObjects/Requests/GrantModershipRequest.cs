namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class GrantModershipRequest : RequestBase
    {
        public int TargetUserId { get; set; }
    }
    public class GrantModershipResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}

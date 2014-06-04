namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class RemoveModershipRequest : RequestBase
    {
        public int TargetUserId { get; set; }
    }
    public class RemoveModershipResponse : ResponseBase
    {
        public bool Success { get; set; }
    }
}
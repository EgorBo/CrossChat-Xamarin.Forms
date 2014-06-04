namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class ResetPhotoRequest : RequestBase
    {
        public int TargetId { get; set; }
    }

    public class ResetPhotoResponse : ResponseBase
    {
        public bool Success { get; set; }
        public int NewPhotoId { get; set; }
    }
}

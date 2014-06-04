namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class SendImageRequest : RequestBase
    {
        public byte[] Image { get; set; }
    }
    public class SendImageResponse : ResponseBase
    {
        public int ImageId { get; set; }
    }
}

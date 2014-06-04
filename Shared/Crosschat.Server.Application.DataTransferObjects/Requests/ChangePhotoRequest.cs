namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class ChangePhotoRequest : RequestBase
    {
        public int BuiltinPhotoId { get; set; }

        public byte[] PhotoData { get; set; }
    }

    public class ChangePhotoResponse : ResponseBase
    {
        public int PhotoId { get; set; }
        public bool Success { get; set; }
    }
}
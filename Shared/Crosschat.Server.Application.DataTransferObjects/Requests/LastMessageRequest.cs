using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class LastMessageRequest : RequestBase { }
    public class LastMessageResponse : ResponseBase
    {
        public string Subject { get; set; }

        public PublicMessageDto[] Messages { get; set; }
    }
}
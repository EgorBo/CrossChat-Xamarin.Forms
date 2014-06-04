namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class BanNotification : BaseDto
    {
        public string TargetName { get; set; }
        public string ActorName { get; set; }
        public string Reason { get; set; }
        public bool Ban { get; set; }
        public int TargetId { get; set; }
    }
}

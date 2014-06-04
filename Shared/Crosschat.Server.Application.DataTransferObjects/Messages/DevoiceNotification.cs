namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class DevoiceNotification : BaseDto
    {
        public string TargetName { get; set; }
        public string ActorName { get; set; }
        public string Reason { get; set; }
        public bool Devoice { get; set; }
        public int TargetId { get; set; }
    }
}
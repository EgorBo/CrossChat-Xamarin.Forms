namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class ModershipGrantedInfo : BaseDto
    {
        public string ActorName { get; set; }
        public string TargetName { get; set; }
        public int TargetId { get; set; }
    }
}
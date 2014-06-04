namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class ModershipRemovedInfo : BaseDto
    {
        public string ActorName { get; set; }
        public string TargetName { get; set; }
        public int TargetId { get; set; }
    }
}

namespace Crosschat.Client.Model.Entities.Messages
{
    public class BanNotificationEvent : Event
    {
        public string TargetName { get; set; }

        public string ActorName { get; set; }

        public string Reason { get; set; }
    }
}
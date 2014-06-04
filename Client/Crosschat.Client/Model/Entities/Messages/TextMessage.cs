using System;

namespace Crosschat.Client.Model.Entities.Messages
{
    public class TextMessage : Event
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsModerator { get; set; }

        public string Body { get; set; }

        public DateTime Timestamp { get; set; }

        public int? ImageId { get; set; }
    }
}

using System;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Domain.Entities
{
    public class PublicMessage : Entity
    {
        public PublicMessage() { }

        public PublicMessage(User author, string body)
        {
            Author = author;
            AuthorName = author.Name;
            Timestamp = DateTime.UtcNow;
            Body = body.CutIfLonger();
            if (string.IsNullOrEmpty(Body))
                throw new ArgumentNullException();
            Timestamp = DateTime.UtcNow;
        }

        public PublicMessage(User author, int imageId, string body)
        {
            Author = author;
            AuthorName = author.Name;
            Timestamp = DateTime.UtcNow;
            Body = body;
            ImageId = imageId;
        }
        
        public virtual User Author { get; private set; }

        public DateTime Timestamp { get; private set; }

        public string AuthorName { get; private set; }

        public string Body { get; private set; }

        public int? ImageId { get; private set; }
    }
}

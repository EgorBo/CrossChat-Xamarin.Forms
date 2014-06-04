using System;
using Crosschat.Client.Model.Entities.Messages;
using Crosschat.Server.Infrastructure;

namespace Crosschat.Client.ViewModels
{
    public class TextMessageViewModel : EventViewModel
    {
        public TextMessageViewModel(TextMessage textMessage, string currentUserName) : base(textMessage)
        {
            AuthorName = textMessage.AuthorName;
            Text = textMessage.Body;
            Timestamp = textMessage.Timestamp;
            IsMine = textMessage.AuthorName == currentUserName;
            ImageId = textMessage.ImageId;
            if (ImageId.HasValue)
            {
                ImageUrl = string.Format("http://{0}/cc/l/{1}.jpg", GlobalConfig.IpAddress, ImageId.Value);
            }
        }

        public int? ImageId { get; set; }

        public string ImageUrl { get; set; }

        public bool IsMine { get; set; }

        public string AuthorName { get; private set; }

        public string Text { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}
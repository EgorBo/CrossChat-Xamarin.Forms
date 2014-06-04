using System;
using Crosschat.Server.Application.DataTransferObjects.Enums;

namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class PublicMessageDto : BaseDto
    {
        //public int AuthorId { get; set; }

        public DateTime Timestamp { get; set; }

        public UserRoleEnum Role { get; set; }

        public string AuthorName { get; set; }

        public string Body { get; set; }

        public int? ImageId { get; set; }
    }
}

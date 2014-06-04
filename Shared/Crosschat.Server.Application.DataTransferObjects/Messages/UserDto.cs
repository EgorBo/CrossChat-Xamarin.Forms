using Crosschat.Server.Application.DataTransferObjects.Enums;

namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class UserDto : BaseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public int Age { get; set; }

        public bool IsDevoiced { get; set; }
        
        public string Platform { get; set; }

        public bool IsBanned { get; set; }

        public string Country { get; set; }

        public UserRoleEnum Role { get; set; }

        public int PhotoId { get; set; }
    }
}

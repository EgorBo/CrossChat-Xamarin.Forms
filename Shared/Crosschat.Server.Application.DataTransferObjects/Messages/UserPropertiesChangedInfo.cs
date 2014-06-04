namespace Crosschat.Server.Application.DataTransferObjects.Messages
{
    public class UserPropertiesChangedInfo : BaseDto
    {
        public int UserId { get; set; } 

        public PropertyValuePair[] Properties { get; set; }
    }
}
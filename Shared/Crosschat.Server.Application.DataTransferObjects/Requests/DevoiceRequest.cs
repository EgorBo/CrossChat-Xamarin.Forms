using Crosschat.Server.Application.DataTransferObjects.Enums;

namespace Crosschat.Server.Application.DataTransferObjects.Requests
{
    public class DevoiceRequest : RequestBase
    {
        /// <summary>
        /// The reason why target has been banned\devoiced
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Target user Id
        /// </summary>
        public int TargetUserId { get; set; }

        /// <summary>
        /// False means to bring voice back for devoices member
        /// </summary>
        public bool Devoice { get; set; }
    }

    public class DevoiceResponse : ResponseBase
    {
        public DevoiceResponseType Result { get; set; }
    }
}
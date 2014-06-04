using System.Collections.Generic;
using System.Linq;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.Sessions;

namespace Crosschat.Server.Application.Services.Helpers
{
    public class ProfileChangesNotificator
    {
        private readonly ISessionManager _sessionManager;

        public ProfileChangesNotificator(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public void NotifyEverybodyInChatAboutProfileChanges(int playerId, Dictionary<string, object> properties)
        {
            if (properties.IsNullOrEmpty())
                return;

            var info = new UserPropertiesChangedInfo();
            info.Properties = properties.Select(i => new PropertyValuePair(i.Key, i.Value)).ToArray();
            info.UserId = playerId;
            _sessionManager.SendToEachChatSessions(info);
        }
    }
}

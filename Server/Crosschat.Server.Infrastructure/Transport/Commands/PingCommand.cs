using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Server.Infrastructure.Transport.Commands
{
    public class PingCommand : CrosschatCommandBase
    {
        protected override bool AllowAnonymousAccess { get { return true; } }

        protected override bool AlwaysAvailable { get { return true; } }

        public override void ExecuteAstralCommand(CrosschatSession session, byte[] data)
        {
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Ping; }
        }
    }
}

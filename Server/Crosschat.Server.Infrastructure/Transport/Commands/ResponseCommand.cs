using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Server.Infrastructure.Transport.Commands
{
    public class ResponseCommand : CrosschatCommandBase
    {
        public override void ExecuteAstralCommand(CrosschatSession session, byte[] data)
        {
            session.AppendResponse(data);
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Response; }
        }
    }
}

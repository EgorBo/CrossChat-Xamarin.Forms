using System;
using System.Linq;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Infrastructure.Protocol;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace Crosschat.Server.Infrastructure.Transport
{
    public class CrosschatReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        private static readonly CommandParser CommandParser = ServiceLocator.Resolve<CommandParser>();

        public CrosschatReceiveFilter() : base(CommandParser.LengthBytesCount + CommandParser.NameBytesCount) { }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return CommandParser.ParseBodyLength(header, offset, length);
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            var commandName = CommandParser.ParseCommandName(header.Array.Skip(header.Offset).Take(CommandParser.NameBytesCount).ToArray());
            return new BinaryRequestInfo(commandName.ToString(), bodyBuffer.CloneRange(offset, length));
        }
    }
}

namespace Crosschat.Server.Infrastructure.Protocol
{
    public class Command
    {
        public CommandNames Name { get; private set; }
        public byte[] Data { get; private set; }

        public Command(CommandNames name, byte[] data)
        {
            Name = name;
            Data = data;
        }
    }
}
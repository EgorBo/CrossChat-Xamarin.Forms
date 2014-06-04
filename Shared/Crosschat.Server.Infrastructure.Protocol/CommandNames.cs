namespace Crosschat.Server.Infrastructure.Protocol
{
    public enum CommandNames
    {
        Unknown = -1,
        Echo = 0,
        Ping,
        Response,
        Request,
        Data,
    }
}

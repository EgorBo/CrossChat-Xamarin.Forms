namespace Crosschat.Server.Application.Contracts
{
    public interface ISettings
    {
        bool IsServerBusy { get; }

        string ServerAddress { get; }

        int ServerPort { get; }

        string Subject { get; }
        
        int LastMessagesCount { get; }

        int ComposeRandomDuelsEachXSeconds { get; }
        
        string ImagesLocalFolder { get; }

        int ThumbnailSize { get; }
        
        int HallOfFameCount { get; }
    }
}

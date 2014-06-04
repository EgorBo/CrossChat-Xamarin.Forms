using System;
using System.Threading.Tasks;

namespace Crosschat.Client.Model.Contracts
{
    public interface ITransportResource
    {
        Task ConnectAsync();

        Task DisconnectAsync();

        event Action ConnectionError;

        event Action<byte[]> DataReceived;

        void SendData(byte[] data);
    }
}
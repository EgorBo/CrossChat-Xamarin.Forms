using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.WinPhone.Infrastructure;
using Crosschat.Server.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(SocketStreamTransportResource))]

namespace Crosschat.Client.WinPhone.Infrastructure
{
    public class SocketStreamTransportResource : ITransportResource
    {
        private StreamSocket _socket;
        private DataWriter _dataWriter;
        private DataReader _dataReader;

        public async Task ConnectAsync()
        {
            IsConnected = false;
            _socket = new StreamSocket();
            await _socket.ConnectAsync(new HostName(GlobalConfig.IpAddress), GlobalConfig.Port.ToString());
            IsConnected = true;
            _dataWriter = new DataWriter(_socket.OutputStream);
            _dataReader = new DataReader(_socket.InputStream) { InputStreamOptions = InputStreamOptions.Partial };
            StartListening();
        }

        private async void StartListening()
        {
            try
            {
                while (true)
                {
                    uint readLength = await _dataReader.LoadAsync(1024);
                    byte[] buffer = new byte[_dataReader.UnconsumedBufferLength];
                    _dataReader.ReadBytes(buffer);
                    DataReceived(buffer);
                }
            }
            catch (Exception)
            {
                if (IsConnected)
                {
                    IsConnected = false;
                    ConnectionError();
                }
            }
        }

        public async Task DisconnectAsync()
        {
            IsConnected = false;
            _socket.Dispose();
            _socket = null;
        }

        public event Action ConnectionError = delegate { };

        public event Action<byte[]> DataReceived = delegate { };

        public bool IsConnected { get; private set; }

        public async void SendData(byte[] bytes)
        {
            try
            {
                _dataWriter.WriteBytes(bytes);
                uint result = await _dataWriter.StoreAsync();
                await _dataWriter.FlushAsync();
            }
            catch (Exception exc)
            {
                if (IsConnected)
                {
                    IsConnected = false;
                    ConnectionError();
                }
            }
        }
    }
}

using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Crosschat.Client.Droid.Infrastructure;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(TransportResource))]

namespace Crosschat.Client.Droid.Infrastructure
{
    public class TransportResource : ITransportResource
    {
        private TcpClient _tcpClient = null;
        private bool _isConnected;
        private const int BufferSize = 8 * 1024;
        private bool _triedConnect = false;

        public event Action ConnectionError = delegate { };
        public event Action<byte[]> DataReceived = delegate { };
        public event Action ConnectionStateChanged = delegate { };
        
        public async Task ConnectAsync()
        {
            IsConnected = false;
            _triedConnect = true;
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(GlobalConfig.IpAddress, GlobalConfig.Port).ConfigureAwait(false);
            StartListening();
            IsConnected = true;
        }

        public async void DropConnection()
        {
            await DisconnectAsync();
            ConnectionError();
        }

        private void StartListening()
        {
            var buffer = new byte[BufferSize];
            try
            {
                _tcpClient.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnDataReceived, buffer);
            }
            catch (Exception exc)
            {
                DropConnection();
            }
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            int dataRead;
            try
            {
                if (!IsConnected || _tcpClient == null || _tcpClient.Client == null)
                {
                    if (IsConnected)
                    {
                        DropConnection();
                    }
                    return;
                }
                 dataRead = _tcpClient.Client.EndReceive(ar);
            }
            catch
            {
                DropConnection();
                return;
            }

            if (dataRead == 0)
            {
                DropConnection();
                return;
            }
            byte[] byteData = ar.AsyncState as byte[];
            DataReceived(byteData);
            StartListening();
        }

        public Task DisconnectAsync()
        {
            if (!_triedConnect)
                return Task.FromResult(false);

            IsConnected = false;
            try
            {
                if (_tcpClient != null && _tcpClient.Connected)
                {
                    _tcpClient.Close();
                }
            }
            catch (Exception exc)
            {
            }
            return Task.FromResult(false);
        }


        public async void SendData(byte[] data)
        {
            try
            {
                if (_tcpClient == null || _tcpClient.Client == null)
                {
                    return;
                }
                await Task.Run(() => _tcpClient.Client.Send(data)).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                DropConnection();
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                if (_isConnected != value)
                {
                    ConnectionStateChanged();
                }
                _isConnected = value;
            }
        }
    }
}

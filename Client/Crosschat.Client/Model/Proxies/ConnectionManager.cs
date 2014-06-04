using System;
using System.Threading;
using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.DataTransferObjects.Utils;
using Crosschat.Server.Infrastructure.Protocol;
using Crosschat.Utils.Tasking;

namespace Crosschat.Client.Model.Proxies
{
    public class ConnectionManager
    {
        private readonly CommandBuffer _commandBuffer;
        private readonly CommandParser _commandParser;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private readonly ITransportResource _transport;
        private readonly RequestsHandler _requestsHandler;
        private readonly IDtoSerializer _serializer;
        private long _lastToken = 1;

        public ConnectionManager(
            ITransportResource transport,
            CommandBuffer commandBuffer,
            CommandParser commandParser,
            RequestsHandler requestsHandler,
            IDtoSerializer serializer)
        {
            _transport = transport;
            _commandBuffer = commandBuffer;
            _commandParser = commandParser;
            _requestsHandler = requestsHandler;
            _serializer = serializer;

            _commandBuffer.CommandAssembled += CommandBuffer_CommandAssembled;
            _transport.DataReceived += data => _commandBuffer.AppendBytes(data);
            _transport.ConnectionError += Transport_ConnectionError;
        }

        private void CommandBuffer_CommandAssembled(Command cmd)
        {
            if (cmd.Name == CommandNames.Response)
            {
                var response = _serializer.Deserialize<ResponseBase>(cmd.Data);
                _requestsHandler.AppendResponse(response);
            }
            else if (cmd.Name == CommandNames.Request)
            {
                var request = _serializer.Deserialize<RequestBase>(cmd.Data);
                RequestReceived(this, new RequestEventArgs(request));
            }
            else if (cmd.Name == CommandNames.Data)
            {
                var dto = _serializer.Deserialize<BaseDto>(cmd.Data);
                DtoReceived(this, new DtoEventArgs(dto));
            }
        }

        private void Transport_ConnectionError()
        {
            IsConnected = false;
            IsConnecting = false;
            ConnectionDropped();
        }

        public event Action ConnectionDropped = delegate { }; 

        public event EventHandler<DtoEventArgs> DtoReceived = delegate { };

        public event EventHandler<RequestEventArgs> RequestReceived = delegate { };

        public async Task ConnectAsync()
        {
            try
            {
                if (IsConnected)
                    return;
                await _semaphoreSlim.WaitAsync();
                if (IsConnected)
                    return;
                IsConnecting = true;
                await _transport.ConnectAsync();
                IsConnecting = false;
                IsConnected = true;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public bool IsConnecting { get; private set; }

        public bool IsConnected { get; private set; }

        internal Task<TResponse> SendRequestAndWaitResponse<TResponse>(RequestBase request) where TResponse : ResponseBase
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
            var requestBytes = _serializer.Serialize(request);
            var command = new Command(CommandNames.Request, requestBytes);
            return _requestsHandler.WaitForResponse<TResponse>(request, () => _transport.SendData(_commandParser.ToBytes(command)));
        }

        internal void SendRequest(RequestBase request)
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
            var requestBytes = _serializer.Serialize(request);
            var command = new Command(CommandNames.Request, requestBytes);
            _transport.SendData(_commandParser.ToBytes(command));
        }

        public async Task DisconnectAsync()
        {
            await _semaphoreSlim.WaitAsync();
            await _transport.DisconnectAsync().WrapWithErrorIgnoring();
            IsConnected = false;
            IsConnecting = false;
            _semaphoreSlim.Release();
        }

        public void SendResponse(ResponseBase response)
        {
            var requestBytes = _serializer.Serialize(response);
            var command = new Command(CommandNames.Response, requestBytes);
            _transport.SendData(_commandParser.ToBytes(command));
        }
    }

    public class DtoEventArgs : EventArgs
    {
        public DtoEventArgs(BaseDto dto)
        {
            Dto = dto;
        }

        public BaseDto Dto { get; private set; }
    }

    public class RequestEventArgs : EventArgs
    {
        public RequestBase Request { get; set; }

        public RequestEventArgs(RequestBase request)
        {
            Request = request;
        }
    }
}

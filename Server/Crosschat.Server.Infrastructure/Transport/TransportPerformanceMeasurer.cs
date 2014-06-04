using System.Threading;

namespace Crosschat.Server.Infrastructure.Transport
{
    public class TransportPerformanceMeasurer
    {
        private long _sentBytes;
        private long _receivedBytes;
        private long _totalConnections;
        private long _sentCommands;
        private long _receivedCommands;
        private long _maxCommandSize;

        public void HandleOutgoingBytes(long count)
        {
            Interlocked.Increment(ref _sentCommands);
            Interlocked.Add(ref _sentBytes, count);
            if (_maxCommandSize < count)
                _maxCommandSize = count;
        }

        public void HandleIncomingBytes(long count)
        {
            Interlocked.Increment(ref _receivedCommands);
            Interlocked.Add(ref _receivedBytes, count);
            if (_maxCommandSize < count)
                _maxCommandSize = count;
        }

        public long MaxCommandSize
        {
            get { return _maxCommandSize; }
        }

        public long SentBytes
        {
            get { return _sentBytes; }
        }

        public long ReceivedBytes
        {
            get { return _receivedBytes; }
        }

        public long SentCommands
        {
            get { return _sentCommands; }
        }

        public long ReceivedCommands
        {
            get { return _receivedCommands; }
        }

        public long TotalConnections
        {
            get { return _totalConnections; }
        }

        public void HandleNewConnection()
        {
            Interlocked.Increment(ref _totalConnections);
        }
    }
}

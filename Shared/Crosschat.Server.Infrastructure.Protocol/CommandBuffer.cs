using System;

namespace Crosschat.Server.Infrastructure.Protocol
{
    public class CommandBuffer
    {
        private readonly CommandParser _commandParser;
        private const int HeaderLength = CommandParser.LengthBytesCount + CommandParser.NameBytesCount;
        private static readonly object SyncObj = new object();
        private byte[] _currentCommand;
        private int _currentCommandExpectedLength;
        private CommandNames _currentCommandName = CommandNames.Unknown;
        private byte[] _headerPartFromPreviousChunk = new byte[0];

        public CommandBuffer(CommandParser commandParser)
        {
            _commandParser = commandParser;
        }

        public event Action<Command> CommandAssembled = delegate { };

        public void AppendBytes(byte[] bytes)
        {
            AppendBytes(bytes, 0, bytes.Length);
        }

        public void AppendBytes(byte[] bytes, int offset, int length)
        {
            lock (SyncObj)
            {
                byte[] bytesToCopy = (offset == 0 && bytes.Length == length) ? bytes : CutSubsequence(bytes, offset, length);
                if (_headerPartFromPreviousChunk.Length > 0)
                {
                    bytesToCopy = ConcatSequences(_headerPartFromPreviousChunk, bytesToCopy);
                    _headerPartFromPreviousChunk = new byte[0];
                }
                if (_currentCommand == null)
                {
                    if (bytesToCopy.Length < HeaderLength + 1)
                    {
                        _headerPartFromPreviousChunk = bytesToCopy;
                        return;
                    }

                    var expLengthBytes = CutSubsequence(bytesToCopy, CommandParser.NameBytesCount, CommandParser.LengthBytesCount);
                    var expLengthInt = BitConverter.ToInt32(CutSubsequence(expLengthBytes, 0, CommandParser.LengthBytesCount), 0);
                    var commandName = _commandParser.ParseCommandName(CutSubsequence(bytesToCopy, 0, CommandParser.NameBytesCount));

                    if (commandName == CommandNames.Unknown)
                    {
                        Clear();
                        //INVALID HEADER
                        return;
                    }

                    if (expLengthInt > 0)
                    {
                        _currentCommandName = commandName;

                        _currentCommandExpectedLength = expLengthInt;
                        if (bytesToCopy.Length - HeaderLength >= _currentCommandExpectedLength)
                        {
                            CommandAssembled(new Command(commandName, CutSubsequence(bytesToCopy, HeaderLength, expLengthInt)));

                            if (bytesToCopy.Length - HeaderLength - _currentCommandExpectedLength > 0)
                            {
                                int newoffset = HeaderLength + _currentCommandExpectedLength;
                                AppendBytes(bytesToCopy, newoffset, bytesToCopy.Length - newoffset);
                                return;
                            }
                        }
                        else
                        {
                            _currentCommand = CutSubsequence(bytesToCopy, HeaderLength, -1);
                        }
                    }
                    else
                    {
                        Clear();
                        //INVALID HEADER
                        return;
                    }
                }
                else
                {
                    _currentCommand = ConcatSequences(_currentCommand, bytesToCopy);
                    if (_currentCommand.Length >= _currentCommandExpectedLength)
                    {
                        var restData = CutSubsequence(_currentCommand, _currentCommandExpectedLength, -1);

                        CommandAssembled(new Command(_currentCommandName, CutSubsequence(_currentCommand, 0, _currentCommandExpectedLength)));

                        _currentCommand = null;
                        _currentCommandExpectedLength = 0;
                        _currentCommandName = CommandNames.Unknown;

                        if (restData.Length > 0)
                        {
                            AppendBytes(restData, 0, restData.Length);
                            return;
                        }
                    }
                }
            }
        }

        private byte[] CutSubsequence(byte[] source, int offset, int count)
        {
            //return source.Skip(offset).Take(count).ToArray();
            if (count == -1)
                count = source.Length - offset;
            var dest = new byte[count];
            Array.Copy(source, offset, dest, 0, count);
            return dest;
        }

        private byte[] ConcatSequences(byte[] array1, byte[] array2)
        {
            //return array1.Concat(array2).ToArray();
            var result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, result, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }

        public void Clear()
        {
            _headerPartFromPreviousChunk = new byte[0];
            _currentCommand = null;
            _currentCommandExpectedLength = 0;
            _currentCommandName = CommandNames.Unknown;
        }
    }
}

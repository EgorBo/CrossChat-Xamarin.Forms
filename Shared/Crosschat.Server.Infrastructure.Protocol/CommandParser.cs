using System;
using System.Collections.Generic;

namespace Crosschat.Server.Infrastructure.Protocol
{   
    /// <summary>
    /// It's a protocol like that:
    /// +-------+--------+-------------------------------+
    /// |request|        |                               |
    /// | name  | len(4) | request body                  |
    /// |  (4)  |        |                               |
    /// +-------+--------+-------------------------------+
    /// request name: the name of the request, 4 chars, used for matching the processing command
    /// request data: the body of the request
    /// TODO: add isEncoded and isZipped flags
    /// </summary>
    public class CommandParser
    {
        public const int LengthBytesCount = 4;
        public const int NameBytesCount = 4;

        public byte[] ToBytes(Command command)
        {
            return ToBytes(command.Name, command.Data);
        }

        public byte[] ToBytes(CommandNames name, byte[] data)
        {
            var result = new List<byte>(data.Length + LengthBytesCount + NameBytesCount); //just an optimization
            result.AddRange(CommandNameToBytes(name));
            result.AddRange(BitConverter.GetBytes(data.Length));
            result.AddRange(data);
            return result.ToArray();
        }
        
        public CommandNames ParseCommandName(byte[] bytes)
        {
            try
            {
                if (bytes.Length != NameBytesCount)
                    throw new ArgumentException();

                var commandIndex = BitConverter.ToInt32(bytes, 0);
                return (CommandNames)commandIndex;
            }
            catch (Exception)
            {
                return CommandNames.Unknown;
            }
        }

        public int ParseBodyLength(byte[] header, int offset, int length)
        {
            var bytes = new[]
                {
                    header[offset + NameBytesCount], 
                    header[offset + NameBytesCount + 1],
                    header[offset + NameBytesCount + 2],
                    header[offset + NameBytesCount + 3]
                };
            var parsedLength = BitConverter.ToInt32(bytes, 0);
            return parsedLength;
        }

        private IEnumerable<byte> CommandNameToBytes(CommandNames name)
        {
            var bytes = new List<byte>(BitConverter.GetBytes((int) name));
            if (bytes.Count > NameBytesCount)
                throw new InvalidOperationException();
            
            while (bytes.Count < NameBytesCount)
            {
                bytes.Insert(0, 0);
            }
            return bytes;
        }
    }
}

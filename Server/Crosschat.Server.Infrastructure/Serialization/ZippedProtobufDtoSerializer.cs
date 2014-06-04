using System.IO;
using System.IO.Compression;

namespace Crosschat.Server.Infrastructure.Serialization
{
    public class ZippedProtobufDtoSerializer : ProtobufDtoSerializer
    {
        public override byte[] Serialize<T>(T obj)
        {
            using (var serializedStream = new MemoryStream())
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                ProtobufModel.Serialize(serializedStream, obj);
                serializedStream.Position = 0;
                serializedStream.CopyTo(zipStream);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public override T Deserialize<T>(byte[] bytes)
        {
            using (var compressedStream = new MemoryStream(bytes))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                resultStream.Position = 0;
                return (T)ProtobufModel.Deserialize(resultStream, null, typeof(T));
            }
        }
    }
}
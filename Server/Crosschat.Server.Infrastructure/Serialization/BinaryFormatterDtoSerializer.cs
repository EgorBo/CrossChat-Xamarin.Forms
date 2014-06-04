using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Server.Infrastructure.Serialization
{
    public class BinaryFormatterDtoSerializer : IDtoSerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new BinaryFormatter();
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
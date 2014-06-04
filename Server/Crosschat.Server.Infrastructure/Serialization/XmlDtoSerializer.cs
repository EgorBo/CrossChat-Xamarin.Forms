using System.IO;
using System.Xml.Serialization;
using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Server.Infrastructure.Serialization
{
    public class XmlDtoSerializer : IDtoSerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(ms);
            }
        }
    }
}
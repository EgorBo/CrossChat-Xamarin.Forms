using System.IO;
using System.Runtime.Serialization.Json;
using Crosschat.Server.Infrastructure.Protocol;

namespace Crosschat.Server.Infrastructure.Serialization
{
    public class JsonDtoSerializer : IDtoSerializer 
    {
        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof (T));
                serializer.WriteObject(ms, obj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}

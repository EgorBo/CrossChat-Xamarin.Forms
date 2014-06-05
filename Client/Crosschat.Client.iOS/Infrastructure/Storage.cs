using System.IO;
using System.Xml.Serialization;
using Crosschat.Client.iOS.Infrastructure;
using Crosschat.Client.Model.Contracts;
using MonoTouch.Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(Storage))]


namespace Crosschat.Client.iOS.Infrastructure
{
    public class Storage : IStorage
    {
        private const string NULL = "!NULL";
        private readonly NSUserDefaults _preferences;

        public Storage()
        {
            _preferences = NSUserDefaults.StandardUserDefaults;
        }

        public void Set<T>(T obj, string key = "")
        {
            key += "0";   
            var str = SerializeToString(obj);
            _preferences.SetString(str ?? NULL, key);
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            field = obj;
            Set(obj, key);
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            key += "0";
            var str = _preferences.StringForKey(key);
            if (str == NULL || string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            var obj = DeserializeFromString<T>(str);
            return obj;
        }

        public static string SerializeToString<T>(T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());
            var textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        public static T DeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return (T)result;
        }
    }
}

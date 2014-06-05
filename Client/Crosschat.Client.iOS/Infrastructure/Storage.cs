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
            var str = ServiceStack.Text.JsonSerializer.SerializeToString(obj);
            _preferences.SetString(str ?? NULL, key);
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            field = obj;
            Set(obj, key);
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            var str = _preferences.StringForKey(key);
            if (str == NULL || string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            var obj = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(str);
            return obj;
        }
    }
}

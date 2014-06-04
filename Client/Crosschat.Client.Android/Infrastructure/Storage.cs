using Android.Content;
using Crosschat.Client.Droid.Infrastructure;
using Crosschat.Client.Model.Contracts;
using Xamarin.Forms;

[assembly: Dependency(typeof(Storage))]


namespace Crosschat.Client.Droid.Infrastructure
{
    public class Storage : IStorage
    {
        private readonly ISharedPreferences _sharedPrefs;

        public Storage()
        {
            _sharedPrefs = Forms.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
        }

        public void Set<T>(T obj, string key = "")
        {
            var edit = _sharedPrefs.Edit();
            string str = ServiceStack.Text.JsonSerializer.SerializeToString(obj);

            edit.PutString(key, str);
            edit.Commit();
        }

        public void Set<T>(ref T field, T obj, string key = "")
        {
            field = obj;
            Set(obj, key);
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            var str = _sharedPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            T value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(str);

            return value;
        }
    }
}

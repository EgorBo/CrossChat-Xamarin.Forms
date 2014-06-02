using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Provider;
using Crosschat.Client.Droid;
using Crosschat.Client.Model;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(AddressbookRepository))]

namespace Crosschat.Client.Droid
{
    public class AddressbookRepository : IAddressbookRepository
    {
        public async Task<Contact[]> GetAllAsync()
        {
            var androidActivity = Xamarin.Forms.Forms.Context as AndroidActivity;
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;

            var contactList = new List<Contact>();
            await Task.Run(() =>
            {
                string[] projection =
                    {
                        ContactsContract.CommonDataKinds.Phone.NormalizedNumber,
                        ContactsContract.CommonDataKinds.Phone.InterfaceConsts.DisplayName,
                        ContactsContract.CommonDataKinds.Phone.InterfaceConsts.PhotoThumbnailUri,
                    };
                var cursor = androidActivity.ManagedQuery(uri, projection, null, null, null);
                if (cursor.MoveToFirst())
                {
                    do
                    {
                        string number = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                        string displayName = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                        string photoUrl = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                        if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(displayName))
                        {
                            contactList.Add(new Contact {Name = displayName, Number = number, PhotoId = photoUrl});
                        }
                    } 
                    while (cursor.MoveToNext());
                }
            });

            if (!contactList.Any())
            {
                return new[]
                        {
                            new Contact { Name = "Egor Bogatov", Number = "+01231"},
                            new Contact { Name = "Ian Gillan", Number = "+01232"},
                            new Contact { Name = "Freddie Mercury", Number = "+01233"},
                            new Contact { Name = "David Gilmour", Number = "+01234"},
                            new Contact { Name = "Steve Ballmer", Number = "+01235"},
                        };
            }

            return contactList.ToArray();
        }
    }
}
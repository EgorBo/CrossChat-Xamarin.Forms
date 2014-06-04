using System.Linq;
using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.WinPhone.Infrastructure;
using Microsoft.Phone.UserData;
using Xamarin.Forms;
using Contact = Crosschat.Client.Model.Entities.Contact;

[assembly: Dependency(typeof(ContactsRepository))]

namespace Crosschat.Client.WinPhone.Infrastructure
{
    public class ContactsRepository : IContactsRepository
    {
        public Task<Contact[]> GetAllAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<Contact[]>();
            var contacts = new Contacts();
            contacts.SearchCompleted += (sender, args) =>
            {
                var addressbook = args.Results
                    .Where(i => !string.IsNullOrEmpty(i.DisplayName))
                    .Select(ToContact)
                    .ToArray();

                //if there is no any contact (run in emulator?) - add several fakes
                if (!addressbook.Any())
                {
                    addressbook = new[]
                        {
                            new Contact { Name = "Egor Bogatov", Number = "+01231"},
                            new Contact { Name = "Ian Gillan", Number = "+01232"},
                            new Contact { Name = "Freddie Mercury", Number = "+01233"},
                            new Contact { Name = "David Gilmour", Number = "+01234"},
                            new Contact { Name = "Steve Ballmer", Number = "+01235"},
                        };
                }

                taskCompletionSource.SetResult(addressbook);
            };
            contacts.SearchAsync(string.Empty, FilterKind.DisplayName, null);

            return taskCompletionSource.Task;
        }

        private Contact ToContact(Microsoft.Phone.UserData.Contact contact)
        {
            string phoneNumber = "no number";
            if (contact.PhoneNumbers != null && contact.PhoneNumbers.Any())
            {
                phoneNumber = contact.PhoneNumbers.First().PhoneNumber;
            }
            return new Contact { Name = contact.DisplayName, Number = phoneNumber };
        }
    }
}

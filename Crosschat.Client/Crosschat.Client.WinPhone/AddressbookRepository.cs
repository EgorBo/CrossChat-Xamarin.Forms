using System.Linq;
using Crosschat.Client.Model;
using System.Threading.Tasks;
using Crosschat.Client.WinPhone;
using Microsoft.Phone.UserData;
using Xamarin.Forms;
using Contact = Crosschat.Client.Model.Contact;


[assembly: Dependency(typeof(AddressbookRepository))]

namespace Crosschat.Client.WinPhone
{
    public class AddressbookRepository : IAddressbookRepository
    {
        public Task<Contact[]> GetAllAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<Contact[]>();
            var contacts = new Contacts();
            contacts.SearchCompleted += (sender, args) => 
                taskCompletionSource.SetResult(args.Results
                    .Where(i => !string.IsNullOrEmpty(i.DisplayName))
                    .Select(ToContact)
                    .ToArray());
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
            return new Contact {Name = contact.DisplayName, Number = phoneNumber};
        }
    }
}

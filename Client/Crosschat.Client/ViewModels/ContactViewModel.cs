using Crosschat.Client.Model.Entities;
using Crosschat.Client.Seedwork;

namespace Crosschat.Client.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private string _number;
        private string _email;
        private string _name;
        private string _photoId;

        public ContactViewModel(Contact contact)
        {
            Contact = contact;
            Number = contact.Number;
            Name = contact.Name;
            PhotoId = contact.PhotoId;
            Email = contact.Email;
        }

        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string PhotoId
        {
            get { return _photoId; }
            set { SetProperty(ref _photoId, value); }
        }

        public Contact Contact { get; private set; }
    }
}

using Crosschat.Client.Model;
using Crosschat.Client.Seedwork;

namespace Crosschat.Client.ViewModels
{
    public class ContactViewModel : ViewModelBase
    {
        private readonly Contact _contact;
        private string _number;
        private string _name;
        private bool _isCrossChatUser;
        private string _photoId;

        public ContactViewModel(Contact contact)
        {
            _contact = contact;
            Number = contact.Number;
            Name = contact.Name;
            PhotoId = contact.PhotoId;
            IsCrossChatUser = true;
        }

        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
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

        public bool IsCrossChatUser
        {
            get { return _isCrossChatUser; }
            set { SetProperty(ref _isCrossChatUser, value); }
        }

        public Contact Contact { get { return _contact; } }
    }
}

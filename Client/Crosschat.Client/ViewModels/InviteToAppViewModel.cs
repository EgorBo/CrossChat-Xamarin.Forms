using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Extensions;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class InviteToAppViewModel : ViewModelBase
    {
        private readonly IContactsRepository _contactsRepository;
        private readonly IContactInvitationSender _invitationSender;
        private ObservableCollection<NamedObservableCollection<ContactViewModel>> _groupedContacts;
        private ObservableCollection<ContactViewModel> _contacts;

        public InviteToAppViewModel()
        {
            _contactsRepository = DependencyService.Get<IContactsRepository>();
            _invitationSender = DependencyService.Get<IContactInvitationSender>();
            ReloadContacts();
        }

        private async void ReloadContacts()
        {
            IsBusy = true;
            var pocoContacts = (await _contactsRepository.GetAllAsync()).ToList();

            Contacts = pocoContacts
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .Select(c => new ContactViewModel(c)).ToObservableCollection();
            GroupedContacts = Contacts.ToNamedObservableCollections(g => g.Name[0].ToString());

            IsBusy = false;
        }

        public ObservableCollection<NamedObservableCollection<ContactViewModel>> GroupedContacts
        {
            get { return _groupedContacts; }
            set { SetProperty(ref _groupedContacts, value); }
        }

        public ObservableCollection<ContactViewModel> Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        public ICommand ContactSelectedCommand
        {
            get { return new Command<ContactViewModel>(OnContactSelected); }
        }

        private void OnContactSelected(ContactViewModel cvm)
        {
            if (cvm != null)
            {
                _invitationSender.Send(cvm.Contact);
            }
        }
    }
}

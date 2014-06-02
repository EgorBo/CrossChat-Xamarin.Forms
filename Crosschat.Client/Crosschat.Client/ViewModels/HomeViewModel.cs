using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Crosschat.Client.Model;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Extensions;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<NamedObservableCollection<ContactViewModel>> _groupedContacts;
        private ObservableCollection<ConversationViewModel> _conversations;
        private readonly IAddressbookRepository _addressbookRepository;
        private readonly MessageService _messageService;
        private ObservableCollection<ContactViewModel> _contacts;

        public HomeViewModel()
        {
            _addressbookRepository = DependencyService.Get<IAddressbookRepository>();
            _messageService = new MessageService();
            Conversations = new ObservableCollection<ConversationViewModel>();
            GroupedContacts = new ObservableCollection<NamedObservableCollection<ContactViewModel>>();
            LoadContacts();
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

        public ObservableCollection<ConversationViewModel> Conversations
        {
            get { return _conversations; }
            set { SetProperty(ref _conversations, value); }
        }

        public ICommand ConversationSelectedCommand
        {
            get { return new Command<ConversationViewModel>(OnConversationSelected); }
        }

        private void OnConversationSelected(ConversationViewModel conversation)
        {
            conversation.ShowAsync();
        }

        private async void OnContactSelected(ContactViewModel contactViewModel)
        {
            if (!contactViewModel.IsCrossChatUser)
            {
                await Notify("", "This contact doesn't have CrossChat installed yet.");
            }
            else
            {
                //look up for an existing conversation with that contact
                var existingConversation = Conversations.FirstOrDefault(c => c.ContactViewModel == contactViewModel);
                if (existingConversation != null)
                {
                    await existingConversation.ShowAsync();
                }
                //create a new one
                else
                {
                    var conversation = new ConversationViewModel(contactViewModel, _messageService);
                    Conversations.Add(conversation);
                    await conversation.ShowAsync();
                }
            }
        }

        private async void LoadContacts()
        {
            IsBusy = true;
            var pocoContacts = (await _addressbookRepository.GetAllAsync()).ToList();
            pocoContacts.Add(new Contact { Name = "Echo1", Number = "+01234567891" });
            pocoContacts.Add(new Contact { Name = "Echo2", Number = "+01234567892" });

            Contacts = pocoContacts
                .Where(c => !string.IsNullOrEmpty(c.Name))
                .Select(c => new ContactViewModel(c)).ToObservableCollection();
            GroupedContacts = Contacts.ToNamedObservableCollections(g => g.Name[0].ToString());

            var lastContactVm = Contacts.Last();
            var builtionConversation = new ConversationViewModel(lastContactVm, _messageService);
            builtionConversation.Messages.Add(new MessageViewModel(new Message { Sender = lastContactVm .Contact, Text = "Welcome to CrossChat!", Timestamp = DateTime.Now}, lastContactVm));
            Conversations.Add(builtionConversation);

            IsBusy = false;
        }
    }
}

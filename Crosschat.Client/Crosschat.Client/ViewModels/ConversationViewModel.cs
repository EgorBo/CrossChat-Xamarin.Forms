using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Crosschat.Client.Model;
using Crosschat.Client.Seedwork;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class ConversationViewModel : ViewModelBase
    {
        private readonly MessageService _messageService;
        private ContactViewModel _contactViewModel;
        private string _inputText;
        private ObservableCollection<MessageViewModel> _messages;

        public ConversationViewModel(ContactViewModel contactViewModel, MessageService messageService)
        {
            _messageService = messageService;
            ContactViewModel = contactViewModel;
            Messages = new ObservableCollection<MessageViewModel>();
            _messageService.MessageReceived += MessageServiceOnMessageReceived;
        }

        public ContactViewModel ContactViewModel
        {
            get { return _contactViewModel; }
            set { SetProperty(ref _contactViewModel, value); }
        }

        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        public string LastMessage
        {
            get { return _messages.Any() ? _messages.Last().Text : "no messages yet"; }
        }

        public ICommand SendMessageCommand
        {
            get { return new Command(OnSendMessage); }
        }

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set
            {
                SetProperty(ref _messages, value);
                if (value != null)
                    value.CollectionChanged += (o, e) => Raise("LastMessage");
            }
        }

        private void OnSendMessage()
        {
            if (string.IsNullOrEmpty(InputText))
                return;

            Messages.Add(new MessageViewModel(_messageService.SendMessage(ContactViewModel.Contact, InputText), new CurrentUserContactViewModel()));
            InputText = string.Empty;
        }

        private void MessageServiceOnMessageReceived(Message message)
        {
            if (ContactViewModel.Number == message.Sender.Number)
            {
                Messages.Add(new MessageViewModel(message, ContactViewModel));
            }
        }
    }
}

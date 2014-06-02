using System;
using Crosschat.Client.Model;
using Crosschat.Client.Seedwork;

namespace Crosschat.Client.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private string _text;
        private DateTime _timestamp;
        private ContactViewModel _senderContactViewModel;

        public MessageViewModel(Message message, ContactViewModel senderContactViewModel)
        {
            SenderContactViewModel = senderContactViewModel;
            Text = message.Text;
            Timestamp = message.Timestamp;
        }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { SetProperty(ref _timestamp, value); }
        }

        public ContactViewModel SenderContactViewModel
        {
            get { return _senderContactViewModel; }
            set { SetProperty(ref _senderContactViewModel, value); }
        }
    }
}

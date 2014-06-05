using System.Collections.ObjectModel;
using System.Windows.Input;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Extensions;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private readonly EventViewModelFactory _eventViewModelFactory;
        private readonly IPhotoPicker _photoPicker;
        private ObservableCollection<UserViewModel> _users;
        private ObservableCollection<EventViewModel> _events;
        private string _inputText;
        private string _subject;

        public HomeViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
            Users = new ObservableCollection<UserViewModel>();
            Events = new ObservableCollection<EventViewModel>();
            _eventViewModelFactory = new EventViewModelFactory();
            _photoPicker = DependencyService.Get<IPhotoPicker>();
            LoadData();
        }

        private async void LoadData()
        {
            IsBusy = true;
            await _appManager.ChatManager.ReloadChat();
            await _appManager.ChatManager.ReloadUsers();
            Subject = _appManager.ChatManager.Subject;

            _appManager.ChatManager.OnlineUsers.SynchronizeWith(Users, u => new UserViewModel(u));
            _appManager.ChatManager.Messages.SynchronizeWith(Events, i => _eventViewModelFactory.Get(i, _appManager.AccountManager.AccountName));
            IsBusy = false;
        }

        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        public ObservableCollection<EventViewModel> Events
        {
            get { return _events; }
            set { SetProperty(ref _events, value); }
        }

        public string InputText
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        public ICommand SendMessageCommand
        {
            get { return new Command(OnSendMessage); }
        }

        public ICommand InviteCommand
        {
            get { return new Command(() => new InviteToAppViewModel().ShowAsync());}
        }

        public ICommand SendImageCommand
        {
            get { return new Command(OnSendImage); }
        }

        private async void OnSendImage()
        {
            var imageData = await _photoPicker.PickPhoto();
            IsBusy = true;
            await _appManager.ChatManager.SendImage(imageData);
            IsBusy = false;
        }

        private void OnSendMessage()
        {
            if (string.IsNullOrEmpty(InputText))
                return;
            string text = InputText;
            InputText = string.Empty;
            _appManager.ChatManager.SendMessage(text);
        }
    }
}

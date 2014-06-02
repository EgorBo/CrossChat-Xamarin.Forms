using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Crosschat.Client.Properties;
using Xamarin.Forms;

namespace Crosschat.Client.Seedwork
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;
        protected static Page _currentPage;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual ViewModelBase SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            return this;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual ViewModelBase Raise(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            return this;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public void OnAppearing(Page page)
        {
            _currentPage = page;
            OnShown();
        }

        protected virtual void OnShown() {}

        public Task ShowAsync()
        {
            //auto-wiring VM with view like MvvmCross and Caliburn do
            string viewModelName = "Crosschat.Client.Views." + GetType().Name.Replace("ViewModel", "") + "Page";
            var page = Activator.CreateInstance(Type.GetType(viewModelName), this) as Page;
            return _currentPage.Navigation.PushAsync(page);
        }

        public ICommand ShowCommand
        {
            get { return new Command(() => ShowAsync());}
        }

        protected Task<bool> Ask(string title, string text)
        {
            if (_currentPage == null)
                return Task.FromResult(false);

            return _currentPage.DisplayAlert(title, text, "Yes", "No");
        }

        protected Task Notify(string title, string text)
        {
            if (_currentPage == null)
                return Task.FromResult(false);

            return _currentPage.DisplayAlert(title, text, "Ok", null);
        }
    }
}

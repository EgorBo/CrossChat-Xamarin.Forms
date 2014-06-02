using System.Linq;
using System.Windows.Input;
using Crosschat.Client.Model;
using Crosschat.Client.Seedwork;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private string _number;
        private string _selectedCountry;
        private readonly CountriesRepository _countriesRepository;

        public RegistrationViewModel()
        {
            _countriesRepository = new CountriesRepository();
        }

        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
        }

        public ICommand RegisterCommand
        {
            get { return new Command(OnRegister); }
        }

        public string[] Countries
        {
            get { return _countriesRepository.GetAll().Select(c => c.ToString()).ToArray(); }
        }

        protected override void OnShown()
        {
            Number = "1234";//TODO: remove
            SelectedCountry = Countries.First();
            base.OnShown();
        }

        private async void OnRegister()
        {
            if (string.IsNullOrEmpty(Number) || SelectedCountry == null)
            {
                await Notify("Invalid data", "Please select a country and enter a valid number");
            }
            else
            {
                await new HomeViewModel().ShowAsync();
            }
        }
    }
}

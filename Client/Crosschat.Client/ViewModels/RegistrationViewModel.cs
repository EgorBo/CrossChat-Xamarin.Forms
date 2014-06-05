using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Crosschat.Client.Model;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Seedwork;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private string _name;
        private string _selectedCountry;
        private readonly CountriesRepository _countriesRepository;
        private string _password;
        private string _selectedAge;
        private string _selectedSex;

        public RegistrationViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
            _countriesRepository = new CountriesRepository();

            SelectedCountry = Countries.First();
            SelectedSex = Sexes.First();
            SelectedAge = Ages.First();
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set { SetProperty(ref _selectedCountry, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string SelectedAge
        {
            get { return _selectedAge; }
            set { SetProperty(ref _selectedAge, value); }
        }

        public string SelectedSex
        {
            get { return _selectedSex; }
            set { SetProperty(ref _selectedSex, value); }
        }

        public ICommand RegisterCommand
        {
            get { return new Command(OnRegister); }
        }

        public string[] Countries
        {
            get { return _countriesRepository.GetAll().Select(c => c.Name).ToArray(); }
        }

        public string[] Ages
        {
            get { return Enumerable.Range(13, 100).Select(i => string.Format("{0} years old", i)).ToArray(); }
        }

        public string[] Sexes
        {
            get { return new[] {"Male", "Female"}; }
        }

        private async void OnRegister()
        {
            if (string.IsNullOrEmpty(Name) || 
                string.IsNullOrEmpty(Password) || 
                string.IsNullOrEmpty(SelectedCountry) ||
                string.IsNullOrEmpty(SelectedAge) ||
                string.IsNullOrEmpty(SelectedSex))
            {
                await Notify("Invalid data", "Please, fill all the fields");
            }
            else
            {
                string platform = Device.OnPlatform("iOS", "Android", "WP8") + (Device.Idiom == TargetIdiom.Tablet ? " Tablet" : "");

                IsBusy = true;
                var registrationResult = await _appManager.AccountManager.Register(Name, Password, ParseAge(SelectedAge), SelectedSex == "Male", SelectedCountry, platform);
                IsBusy = false;

                if (registrationResult == RegistrationResponseType.Success)
                {
                    await new HomeViewModel(_appManager).ShowAsync();
                }
                else if (registrationResult == RegistrationResponseType.InvalidData)
                {
                    await Notify("Invalid data", "Please, fill all the fields");
                }
                else if (registrationResult == RegistrationResponseType.NameIsInUse)
                {
                    await Notify("Invalid data", "This name is already taken by someone else");
                }
            }
        }

        private static int ParseAge(string selectedAge)
        {
            return int.Parse(selectedAge.Split(' ')[0]);
        }
    }
}

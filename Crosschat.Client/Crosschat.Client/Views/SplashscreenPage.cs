using System.Threading.Tasks;
using Crosschat.Client.ViewModels;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class SplashscreenPage : ContentPage
    {
        public SplashscreenPage()
        {
            Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                        {
                            new Label
                                {
                                    Text = "Connecting...", 
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    Font = Font.BoldSystemFontOfSize(24),
                                },
                            new ActivityIndicator 
                                {
                                    IsRunning = true, 
                                }
                        }
                };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            this.Title = "Cross Chat";
        }

        protected override async void OnAppearing()
        {
            //await Task.Delay(2000); //simulate connecting to a server
            await Navigation.PushAsync(new RegistrationPage(new RegistrationViewModel()));
            //await Navigation.PushAsync(new ContactsPage(HomeViewModel.Instance));
            base.OnAppearing();
        }
    }
}

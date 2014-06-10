using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Seedwork;
using Crosschat.Client.ViewModels;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Infrastructure.Protocol;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class SplashscreenPage : ContentPage
    {
        private static ApplicationManager _applicationManager;

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
            
            //NOTE: this button is a workaround, adding button on HomePage doesn't work so it will be presented always
            //but will work only in Chat
            var sendImageItem = new ToolbarItem("send photo", Device.OnPlatform(null, null, "appbar.image.beach.png"),
                () =>
                {
                    var homeVm = ViewModelBase.CurrentViewModel as HomeViewModel;
                    if (homeVm != null)
                    {
                        homeVm.SendImageCommand.Execute(null);
                    }
                    else if (ViewModelBase.CurrentViewModel != null)
                    {
                        ViewModelBase.CurrentViewModel.Notify(";(", "You can send images only in chat. I just don't know how to show it only on specific pages - ToolbarItems.Add doesn't work on HomePage ;(");
                    }
                });
            sendImageItem.SetBinding(ToolbarItem.CommandProperty, new Binding("SendImageCommand"));
            Device.OnPlatform(WinPhone: () => ToolbarItems.Add(sendImageItem)); 

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            this.Title = "Cross Chat";
        }

        protected override async void OnAppearing()
        {
            if (_applicationManager == null)
            {
                _applicationManager = new ApplicationManager(
                    DependencyService.Get<ITransportResource>(),
                    DependencyService.Get<IDtoSerializer>(),
                    DependencyService.Get<IStorage>(),
                    DependencyService.Get<IDeviceInfo>());
                _applicationManager.ConnectionManager.ConnectionDropped += () => Navigation.PushAsync(new SplashscreenPage());
            }

            AuthenticationResponseType result;
            try
            {
                result = await _applicationManager.AccountManager.ValidateAccount();
            }
            catch (System.Exception)
            {
                DisplayAlert(";(", "Server is not available", "Ok", null);
                return;
            }
            if (result == AuthenticationResponseType.Success)
            {
                await Navigation.PushAsync(new HomePage(new HomeViewModel(_applicationManager)));
            }
            else
            {
                await Navigation.PushAsync(new RegistrationPage(new RegistrationViewModel(_applicationManager)));
            }

            base.OnAppearing();
        }

    }
}

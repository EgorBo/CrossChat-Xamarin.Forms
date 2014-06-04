using Crosschat.Client.ViewModels;
using Crosschat.Client.Views;
using Xamarin.Forms;

namespace Crosschat.Client
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new NavigationPage(new SplashscreenPage());
        }
    }
}

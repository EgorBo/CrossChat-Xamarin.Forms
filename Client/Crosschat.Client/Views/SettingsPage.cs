using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class SettingsPage : MvvmableContentPage
    {
        public SettingsPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Settings";
            Icon = "settings.png";

            var inviteButton = new Button();
            inviteButton.Text = "Invite contacts";
            inviteButton.SetBinding(Button.CommandProperty, new Binding("InviteCommand"));

            Content = new StackLayout
            {
                Children =
                {
                    inviteButton
                }
            };
        }
    }
}

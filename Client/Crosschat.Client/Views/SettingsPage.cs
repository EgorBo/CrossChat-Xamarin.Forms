using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class SettingsPage : MvvmableContentPage
    {
        public SettingsPage(ViewModelBase viewModel)
            : base(viewModel)
        {
            Title = "Settings";

            Button sendInviteButton = new Button();
            sendInviteButton.Text = "Invite my contacts";
            sendInviteButton.SetBinding(Button.CommandProperty, new Binding("InviteCommand"));

            Content = new StackLayout
                {
                    Children =
                        {
                            sendInviteButton
                        }
                };
        }
    }
}

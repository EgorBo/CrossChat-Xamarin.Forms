using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;

namespace Crosschat.Client.Views
{
    public class HomePage : MvvmableTabbedPage
    {
        public HomePage(ViewModelBase viewModel) : base(viewModel)
        {
            Children.Add(new ChatPage(viewModel));
            Children.Add(new OnlineUsersPage(viewModel));
            Children.Add(new SettingsPage(viewModel));
        }
    }
}

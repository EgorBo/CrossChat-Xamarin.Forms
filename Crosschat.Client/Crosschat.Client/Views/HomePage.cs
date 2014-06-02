using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;

namespace Crosschat.Client.Views
{
    public class HomePage : MvvmableTabbedPage
    {
        public HomePage(ViewModelBase viewModel) : base(viewModel)
        {
            Children.Add(new ContactsPage(viewModel));
            Children.Add(new ConversationsPage(viewModel));
        }
    }
}

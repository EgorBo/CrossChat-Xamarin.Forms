using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class ConversationsPage : MvvmableContentPage
    {
        public ConversationsPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Conversations";

            var conversationsList = new BindableListView();
            conversationsList.ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, "ContactViewModel.Name");
                    cell.SetBinding(TextCell.DetailProperty, "LastMessage");
                    return cell;
                });
            conversationsList.SetBinding(ListView.ItemsSourceProperty, new Binding("Conversations"));
            conversationsList.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("ConversationSelectedCommand"));
            Content = conversationsList;
        }
    }
}
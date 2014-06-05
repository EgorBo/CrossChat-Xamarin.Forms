using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class InviteToAppPage : MvvmableContentPage
    {
        public InviteToAppPage(ViewModelBase viewModel) : base(viewModel)
        {
            Title = "Contacts";

            var contactsCountLabel = new Label();
            contactsCountLabel.SetBinding(Label.TextProperty, new Binding("Contacts.Count", stringFormat: "{0} contacts."));

            var tipLabel = new Label();
            tipLabel.Text = "Select a contact to send an invitation";

            var listView = new BindableListView
                {
                    ItemTemplate = new DataTemplate(() =>
                        {
                            var imageCell = new ImageCell
                                {
                                    ImageSource = Device.OnPlatform(
                                        ImageSource.FromFile("empty_contact.jpg"),
                                        ImageSource.FromFile("empty_contact.jpg"),
                                        ImageSource.FromFile("Assets/empty_contact.jpg")),
                                };
                            imageCell.SetBinding(TextCell.TextProperty, new Binding("Name"));
                            imageCell.SetBinding(TextCell.DetailProperty, new Binding("Number"));
                            return imageCell;
                        }),
                    IsGroupingEnabled = true,
                    GroupDisplayBinding = new Binding("Name"),
                };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("GroupedContacts"));
            listView.SetBinding(BindableListView.ItemClickedCommandProperty, new Binding("ContactSelectedCommand"));

            var contactsLoadingIndicator = new ActivityIndicator();
            contactsLoadingIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));

            Content = new StackLayout
                {
                    Children =
                        {
                            contactsLoadingIndicator,
                            contactsCountLabel,
                            tipLabel,
                            listView
                        }
                };
        }
    }
}

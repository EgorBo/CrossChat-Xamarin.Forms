using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Crosschat.Client.Views.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class ChatPage : MvvmableContentPage
    {
        public ChatPage(ViewModelBase viewModel) : base(viewModel)
        {

            Title = "Chat";

            var headerLabel = new Label();
            headerLabel.Font = Font.BoldSystemFontOfSize(24);
            headerLabel.TextColor = Color.Yellow;
            headerLabel.SetBinding(Label.TextProperty, new Binding("Subject"));

            var sendButton = new Button();
            sendButton.Text = " Send ";
            sendButton.VerticalOptions = LayoutOptions.EndAndExpand;
            sendButton.SetBinding(Button.CommandProperty, new Binding("SendMessageCommand"));
            sendButton.BackgroundColor = Color.Green;
            sendButton.BorderColor = Color.Green;

            var inputBox = new Entry();
            inputBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            inputBox.Keyboard = Keyboard.Chat;
            inputBox.Placeholder = "Type a message...";
            inputBox.SetBinding(Entry.TextProperty, new Binding("InputText", BindingMode.TwoWay));

            var messageList = new ListView();
            messageList.VerticalOptions = LayoutOptions.FillAndExpand;
            messageList.SetBinding(ChatListView.ItemsSourceProperty, new Binding("Events"));
            messageList.ItemTemplate = new DataTemplate(() =>
            {
                var cell = new TextCell();
                cell.TextColor = Color.Yellow;
                cell.SetBinding(TextCell.TextProperty, new Binding("AuthorName", stringFormat: "{0}:"));
                cell.SetBinding(TextCell.DetailProperty, new Binding("Text"));
                return cell;

                /*NOTE: this pretty datatemplate works incorrectly for Android
                var timestampLabel = new Label();
                timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
                timestampLabel.TextColor = Color.Silver;
                timestampLabel.Font = Font.SystemFontOfSize(20);

                var authorLabel = new Label();
                authorLabel.SetBinding(Label.TextProperty, new Binding("AuthorName", stringFormat: "{0}: "));
                authorLabel.TextColor = Color.Yellow;
                authorLabel.Font = Font.SystemFontOfSize(20);

                var messageLabel = new Label();
                messageLabel.SetBinding(Label.TextProperty, new Binding("Text"));
                messageLabel.Font = Font.SystemFontOfSize(20);

                var view = new ViewCell
                    {
                        View = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = { timestampLabel, authorLabel, messageLabel }
                            }
                    };
                return view;*/
            });
            
            Content = new StackLayout
                {
                    Children =
                        {
                            new StackLayout
                                {
                                    Children = {inputBox, sendButton},
                                    Orientation = StackOrientation.Horizontal,
                                    Padding = new Thickness(0, Device.OnPlatform(0, 20, 0),0,0),
                                },
                            headerLabel,
                            messageList,
                        }
                };
        }
    }
}

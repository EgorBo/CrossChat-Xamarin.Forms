using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Controls;
using Crosschat.Client.Views.Controls;
using Xamarin.Forms;

namespace Crosschat.Client.Views
{
    public class ConversationPage : MvvmableContentPage
    {
        public ConversationPage(ViewModelBase viewModel) : base(viewModel)
        {
            var headerLabel = new Label { Font = Font.BoldSystemFontOfSize(50) };
            headerLabel.SetBinding(Label.TextProperty, new Binding("ContactViewModel.Name"));

            var sendButton = new Button { Text = "Send" };
            sendButton.SetBinding(Button.CommandProperty, new Binding("SendMessageCommand"));

            var inputBox = new Entry { HorizontalOptions = LayoutOptions.FillAndExpand, Keyboard = Keyboard.Chat };
            inputBox.SetBinding(Entry.TextProperty, new Binding("InputText", BindingMode.TwoWay));
            
            var messageList = new ListView();
            messageList.VerticalOptions = LayoutOptions.FillAndExpand;
            messageList.SetBinding(ListView.ItemsSourceProperty, new Binding("Messages"));
            messageList.ItemTemplate = new DataTemplate(() =>
                {
                    var timestampLabel = new Label();
                    timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
                    timestampLabel.TextColor = Color.Silver;

                    var authorLabel = new Label();
                    authorLabel.SetBinding(Label.TextProperty, new Binding("SenderContactViewModel.Name", stringFormat: "{0}: "));
                    authorLabel.TextColor = Color.Yellow;

                    var messageLabel = new Label();
                    messageLabel.SetBinding(Label.TextProperty, new Binding("Text"));

                    var view = new MessageViewCell
                        {
                            View = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Children = { timestampLabel, authorLabel, messageLabel }
                                }
                        };
                    return view;
                });

            Content = new StackLayout
                {
                    Children =
                        {
                            headerLabel,
                            messageList,
                            new StackLayout
                                {
                                    Children = {inputBox, sendButton},
                                    Orientation = StackOrientation.Horizontal,
                                },
                        }
                };
        }
    }
}

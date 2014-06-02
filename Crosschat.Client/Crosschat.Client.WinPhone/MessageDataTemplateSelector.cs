using System.Windows;
using System.Windows.Controls;
using Crosschat.Client.ViewModels;

namespace Crosschat.Client.WinPhone
{
    public class MessageDataTemplateSelector : ContentControl
    {
        public DataTemplate MyMessageTemplate { get; set; }

        public DataTemplate OpponentTemplate { get; set; }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            var message = newContent as MessageViewModel;
            ContentTemplate = message.SenderContactViewModel is CurrentUserContactViewModel ? MyMessageTemplate : OpponentTemplate;
        }
    }
}
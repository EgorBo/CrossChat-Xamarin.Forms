using System;
using System.Windows;
using System.Windows.Controls;
using Crosschat.Client.ViewModels;

namespace Crosschat.Client.WinPhone
{
    public class MessageDataTemplateSelector : ContentControl
    {
        public DataTemplate MyMessageTemplate { get; set; }

        public DataTemplate OpponentTemplate { get; set; }

        public DataTemplate MyImageTemplate { get; set; }

        public DataTemplate OpponentImageTemplate { get; set; }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            var eventVm = newContent as EventViewModel;
            var textMsgVm = eventVm as TextMessageViewModel;
            if (textMsgVm != null)
            {
                if (textMsgVm.ImageId.HasValue)
                {
                    ContentTemplate = textMsgVm.IsMine ? MyImageTemplate : OpponentImageTemplate;
                }
                else
                {
                    ContentTemplate = textMsgVm.IsMine ? MyMessageTemplate : OpponentTemplate;
                }
            }
            
        }
    }
}
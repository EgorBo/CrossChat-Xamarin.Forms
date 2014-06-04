using System.Windows;
using Crosschat.Client.Views.Controls;
using Crosschat.Client.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using DataTemplate = System.Windows.DataTemplate;

[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace Crosschat.Client.WinPhone.CustomRenderers
{

    public class MessageRenderer : ViewCellRenderer
    {
        public override DataTemplate GetTemplate(Cell cell)
        {
            return Application.Current.Resources["MessageDataTemplate"] as DataTemplate;
        }
    }
}
 
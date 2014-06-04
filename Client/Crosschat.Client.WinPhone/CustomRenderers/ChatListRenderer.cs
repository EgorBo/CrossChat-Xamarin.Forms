using System.Collections.Specialized;
using System.Linq;
using Crosschat.Client.Views.Controls;
using Crosschat.Client.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ChatListView), typeof(ChatListRenderer))]

namespace Crosschat.Client.WinPhone.CustomRenderers
{
    public class ChatListRenderer : ListViewRenderer
    {
        protected override void OnModelSet()
        {
            base.OnModelSet();
            var control = Control;
            if (control != null && control.ItemsSource is INotifyCollectionChanged)
            {
                //auto-scroll on ItemsSource change
                ((INotifyCollectionChanged) control.ItemsSource).CollectionChanged += (sender, args) => 
                    Dispatcher.BeginInvoke(()=> control.ScrollTo(control.ItemsSource.OfType<object>().FirstOrDefault()));
            }
        }
    }
}
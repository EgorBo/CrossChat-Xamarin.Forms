using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Crosschat.Client.Droid.CustomRenderers;
using Crosschat.Client.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

//[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageRenderer))]

namespace Crosschat.Client.Droid.CustomRenderers
{
    //public class MessageRenderer : ViewCellRenderer
    //{
    //    protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
    //    {
    //        var view = base.GetCellCore(item, convertView, parent, context);
    //        var activity = ((Activity) Forms.Context);
    //        //var layout = activity.FindViewById(Resource.Layout.message_item);
    //        ////TODO: apply layout

    //        return view;
    //    }

    //    protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnCellPropertyChanged(sender, e);
    //    }
    //}
}
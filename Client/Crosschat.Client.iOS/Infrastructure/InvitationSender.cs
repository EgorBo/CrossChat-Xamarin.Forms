using Crosschat.Client.iOS.Infrastructure;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Entities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(InvitationSender))]

namespace Crosschat.Client.iOS.Infrastructure
{
    public class InvitationSender : IContactInvitationSender
    {
        public void Send(Contact contact)
        {
            var smsTo = NSUrl.FromString("sms:" + contact.Number);
            UIApplication.SharedApplication.OpenUrl(smsTo);
        }
    }
}

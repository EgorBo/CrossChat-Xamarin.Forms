using System;
using System.Threading.Tasks;

namespace Crosschat.Client.Model
{
    public class MessageService
    {
        public Message SendMessage(Contact reciever, string message)
        {
            var myMessage = new Message { Text = message, Timestamp = DateTime.Now };
            
            //echo the message
            var echoMessage = new Message { Text = message, Timestamp = DateTime.Now, Sender = reciever};
            Task.Delay(1000).ContinueWith(r => MessageReceived(echoMessage), TaskContinuationOptions.ExecuteSynchronously);
            
            return myMessage;
        }

        public event Action<Message> MessageReceived = delegate { };
    }
}

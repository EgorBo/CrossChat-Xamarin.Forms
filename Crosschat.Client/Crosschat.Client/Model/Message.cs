using System;

namespace Crosschat.Client.Model
{
    public class Message
    {
        public string Text { get; set; }
        
        public Contact Sender { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

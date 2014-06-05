using Crosschat.Client.Model.Entities.Messages;

namespace Crosschat.Client.ViewModels
{
    public class EventViewModelFactory
    {
        public EventViewModel Get(Event @event, string currentUserName)
        {
            if (@event is TextMessage)
                return new TextMessageViewModel(@event as TextMessage, currentUserName);

            //TODO: create VM for other event types 

            return new EventViewModel(@event);
        }
    }
}

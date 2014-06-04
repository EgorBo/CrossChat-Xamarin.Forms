using Crosschat.Client.Model.Entities.Messages;
using Crosschat.Client.Seedwork;

namespace Crosschat.Client.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private readonly Event _eventPoco;

        public EventViewModel(Event eventPoco)
        {
            _eventPoco = eventPoco;
        }
    }
}

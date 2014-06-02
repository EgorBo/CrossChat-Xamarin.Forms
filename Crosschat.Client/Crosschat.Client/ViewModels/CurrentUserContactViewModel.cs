using Crosschat.Client.Model;

namespace Crosschat.Client.ViewModels
{
    public class CurrentUserContactViewModel : ContactViewModel
    {
        public CurrentUserContactViewModel() : base(new Contact { Name = "Me"}) {}
    }
}
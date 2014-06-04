using Crosschat.Client.Seedwork;
using Crosschat.Server.Application.DataTransferObjects.Messages;

namespace Crosschat.Client.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly UserDto _userDto;
        private string _description;
        private string _name;

        public UserViewModel(UserDto userDto)
        {
            _userDto = userDto;
            Name = userDto.Name;
            Description = string.Format("{0}, {1}", userDto.Country, userDto.Platform);
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}

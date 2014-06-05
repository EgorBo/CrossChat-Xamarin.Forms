using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Crosschat.Client.Model.Proxies;
using Crosschat.Client.Seedwork.Extensions;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Client.Model.Managers
{
    public class FriendsManager : ManagerBase
    {
        private readonly FriendsServiceProxy _friendsServiceProxy;

        public FriendsManager(ConnectionManager connectionManager, FriendsServiceProxy friendsServiceProxy)
            : base(connectionManager)
        {
            _friendsServiceProxy = friendsServiceProxy;
            Friends = new ObservableCollection<UserDto>();
        }

        /// <summary>
        /// Active friends
        /// </summary>
        public ObservableCollection<UserDto> Friends { get; private set; }

        /// <summary>
        /// Reloads Friends collection
        /// </summary>
        public async Task ReloadFriendsAsync()
        {
            var response = await _friendsServiceProxy.GetFriends(new UserFriendsRequest());
            Friends.Clear();
            if (response.Friends != null)
            {
                Friends.AddRange(response.Friends);
            }
        }

        /// <summary>
        /// Adds specified player to friends list
        /// </summary>
        public async Task<bool> AddToFriendsAsync(UserDto player)
        {
            var result = await _friendsServiceProxy.AddToFriends(new AddToFriendsRequest { TargetUserId = player.Id });
            if (!result.Success)
            {
                return false;
            }
            Friends.Add(player);
            return true;
        }

        /// <summary>
        /// Removes specified player from friends list
        /// </summary>
        public async Task<bool> RemoveFromFriendsAsync(UserDto player)
        {
            var result = await _friendsServiceProxy.RemoveFromFriends(new RemoveFromFriendsRequest { TargetUserId = player.Id });
            if (!result.Success)
            {
                return false;
            }
            RemoveEntityFromList(Friends, i => i.Id == player.Id);
            return true;
        }

        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            var playerProfileChanges = dto as UserPropertiesChangedInfo;
            if (playerProfileChanges != null)
            {
                UpdatePropertiesForList(Friends, p => p.Id == playerProfileChanges.UserId, playerProfileChanges.Properties);
            }

            base.OnUnknownDtoReceived(dto);
        }
    }
}
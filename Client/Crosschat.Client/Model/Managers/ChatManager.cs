using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Crosschat.Client.Model.Entities.Messages;
using Crosschat.Client.Model.Proxies;
using Crosschat.Client.Seedwork.Extensions;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Client.Model.Managers
{
    public class ChatManager : ManagerBase
    {
        private readonly AccountManager _accountManager;
        private readonly ChatServiceProxy _chatServiceProxy = null;
        private string _subject;

        public ChatManager(
            ConnectionManager connectionManager,
            ChatServiceProxy chatServiceProxy,
            AccountManager accountManager)
            : base(connectionManager)
        {
            _chatServiceProxy = chatServiceProxy;
            _accountManager = accountManager;

            Messages = new ObservableCollection<Event>();
            OnlineUsers = new ObservableCollection<UserDto>();
        }

        /// <summary>
        /// Reloads only messages and subject
        /// </summary>
        public async Task ReloadChat()
        {
            var chatStatus = await _chatServiceProxy.GetLastMessages(new LastMessageRequest());
            Subject = chatStatus.Subject;
            Messages.Clear();
            if (chatStatus.Messages != null)
            {
                Messages.AddRange(chatStatus.Messages.Select(ToEntity<TextMessage>));
            }
        }

        /// <summary>
        /// Reloads only online players
        /// </summary>
        public async Task ReloadUsers()
        {
            var chatStatus = await _chatServiceProxy.GetOnlineUsers(new GetOnlineUsersRequest());
            OnlineUsers.Clear();
            if (chatStatus.Users != null)
            {
                OnlineUsers.AddRange(chatStatus.Users);
            }

            //for screenshots ;)
            /*OnlineUsers.Add(new UserDto { Name = "Tim Cook", Country = "United States", Platform = "iOS" });
            OnlineUsers.Add(new UserDto { Name = "Eric Schmidt", Country = "United States", Platform = "Android" });
            OnlineUsers.Add(new UserDto { Name = "Satya Nadella", Country = "United States", Platform = "WP8" });
            OnlineUsers.Add(new UserDto { Name = "Miguel de Icaza", Country = "United States", Platform = "iOS" });
            OnlineUsers.Add(new UserDto { Name = "Egor Bogatov", Country = "Belarus", Platform = "Nokia 3310" });*/
        }

        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> Ban(int playerId, string reason)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { Reason = reason, TargetUserId = playerId, Ban = true });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> UnBan(int playerId)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { TargetUserId = playerId });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> Devoice(int playerId, string reason)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { Reason = reason, TargetUserId = playerId, Devoice = true });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> BringVoiceBack(int playerId)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { TargetUserId = playerId });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Reset photo (only for moders)
        /// </summary>
        public async Task<bool> ResetPhoto(int playerId)
        {
            var result = await _chatServiceProxy.ResetPhoto(new ResetPhotoRequest { TargetId = playerId });
            return result.Success;
        }

        /// <summary>
        /// Send a public message
        /// </summary>
        public void SendMessage(string message)
        {
            _chatServiceProxy.PublicMessage(new PublicMessageRequest { Body = message });
        }

        public Task SendImage(byte[] image)
        {
            return _chatServiceProxy.SendImage(new SendImageRequest { Image = image });
        }

        /// <summary>
        /// Fires on subject change
        /// </summary>
        public event EventHandler SubjectChanged = delegate { };

        /// <summary>
        /// Chat topic (subject)
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            private set
            {
                _subject = value;
                SubjectChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<Event> Messages { get; private set; }

        public ObservableCollection<UserDto> OnlineUsers { get; private set; }

        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            //messages:
            ToEntityAndAddToList<BanNotificationEvent, Event, BanNotification>(dto, Messages, null, false);
            ToEntityAndAddToList<TextMessage, Event, PublicMessageDto>(dto, Messages, null, false);
            ToEntityAndAddToList<DevoiceNotificationEvent, Event, DevoiceNotification>(dto, Messages, null, false);
            ToEntityAndAddToList<GrantedModershipNotificationEvent, Event, ModershipGrantedInfo>(dto, Messages, null, false);
            ToEntityAndAddToList<RemovedModershipNotificationEvent, Event, ModershipRemovedInfo>(dto, Messages, null, false);

            var userDto = dto as UserDto;
            if (userDto != null)
                OnlineUsers.Add(userDto);

            var joinedUserDto = dto as JoinedUserInfo;
            if (joinedUserDto != null)
                OnlineUsers.Add(joinedUserDto.User);

            var leftUserInfo = dto as LeftUserInfo;
            if (leftUserInfo != null)
                RemoveEntityFromList(OnlineUsers, i => i.Id == leftUserInfo.UserId);

            var playerProfileChanges = dto as UserPropertiesChangedInfo;
            if (playerProfileChanges != null)
            {
                UpdatePropertiesForList(OnlineUsers, p => p.Id == playerProfileChanges.UserId, playerProfileChanges.Properties);
            }

            //update property IsDevoiced for players
            var devoiceNotification = dto as DevoiceNotification;
            if (devoiceNotification != null)
            {
                var player = OnlineUsers.FirstOrDefault(i => i.Id == devoiceNotification.TargetId);
                if (player != null)
                    player.IsDevoiced = devoiceNotification.Devoice;
            }

            var youAreDevoicedNotification = dto as YouAreDevoicedNotification;
            if (youAreDevoicedNotification != null)
            {
                //TODO: Notify current user
            }
        }
    }
}

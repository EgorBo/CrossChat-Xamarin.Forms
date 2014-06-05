using System.Collections.Generic;
using System.Linq;
using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Services.Helpers;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Exceptions;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Application.Services
{
    public class ChatService : AppService
    {
        private readonly ISessionManager _sessionManager;
        private readonly ISettings _settings;
        private readonly IFileStorage _fileStorage;
        private readonly ProfileChangesNotificator _profileChangesNotificator;

        public ChatService(
            ISessionManager sessionManager,
            ISettings settings,
            IFileStorage fileStorage,
            ProfileChangesNotificator profileChangesNotificator,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(unitOfWorkFactory)
        {
            _sessionManager = sessionManager;
            _settings = settings;
            _fileStorage = fileStorage;
            _profileChangesNotificator = profileChangesNotificator;
            _sessionManager.AuthenticatedUserConnected += SessionManagerOnAuthenticatedUserConnected;
            _sessionManager.AuthenticatedUserDisconnected += SessionManagerOnAuthenticatedUserDisconnected;
        }

        /// <summary>
        /// NOTE: we don't need an app-level ack for this method :-)
        /// </summary>
        public void PublicMessage(ISession session, PublicMessageRequest msgRequest)
        {
            if (session.User.IsDevoiced)
            {
                session.Send(new YouAreDevoicedNotification());
                return;
            }

            PublicMessage message = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                message = new PublicMessage(session.User, msgRequest.Body);
                uow.PublicMessageRepository.Add(message);
                uow.Commit();
            }

            var dto = message.ProjectedAs<PublicMessageDto>();
            dto.Role = (UserRoleEnum)(int)session.User.Role;
            _sessionManager.SendToEachChatSessions(dto);
        }

        public SendImageResponse SendImage(ISession session, SendImageRequest request)
        {
            var response = request.CreateResponse<SendImageResponse>();
            response.ImageId = -1;

            if (session.User.IsDevoiced)
            {
                session.Send(new YouAreDevoicedNotification());
                return response;
            }

            PublicMessage message = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                int fileId;
                try
                {
                    fileId = _fileStorage.AppendFile(request.Image);
                    response.ImageId = fileId;
                }
                catch (System.Exception)
                {
                    //Invalid image
                    return response;
                }
                uow.Attach(session.User);
                message = new PublicMessage(session.User, fileId, "Your client doesn't support IMAGE messages");
                uow.PublicMessageRepository.Add(message);
                uow.Commit();
            }

            var dto = message.ProjectedAs<PublicMessageDto>();
            dto.Role = (UserRoleEnum)(int)session.User.Role;
            _sessionManager.SendToEachChatSessions(dto);
            return response;
        }

        public GrantModershipResponse GrantModership(ISession session, GrantModershipRequest request)
        {
            var response = request.CreateResponse<GrantModershipResponse>();
            if (session.User.Role != UserRole.Admin)
            {
                //access denied
                return response;
            }

            User targetUser = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser == null)
                {
                    //User not found
                    return response;
                }

                targetUser.RemoveModeratorship();
                uow.Commit();
            }

            response.Success = true;
            _sessionManager.SendToEachChatSessions(new ModershipGrantedInfo
                {
                    ActorName = session.User.Name,
                    TargetName = targetUser.Name,
                    TargetId = targetUser.Id,
                });

            //success
            return response;
        }

        public RemoveModershipResponse RemoveModership(ISession session, RemoveModershipRequest request)
        {
            var response = request.CreateResponse<RemoveModershipResponse>();
            if (session.User.Role != UserRole.Admin)
            {
                //access denied
                return response;
            }

            User targetUser = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser == null)
                {
                    //User not found
                    return response;
                }

                targetUser.RemoveModeratorship();
                uow.Commit();
            }

            response.Success = true;
            _sessionManager.SendToEachChatSessions(new ModershipRemovedInfo
                {
                    ActorName = session.User.Name,
                    TargetName = targetUser.Name,
                    TargetId = targetUser.Id,
                });

            //success
            return response;
        }

        public DevoiceResponse Devoice(ISession session, DevoiceRequest request)
        {
            var response = request.CreateResponse<DevoiceResponse>();

            User targetUser = null;

            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser != null)
                {
                    try
                    {
                        if (request.Devoice)
                        {
                            targetUser.Devoice(session.User);
                        }
                        else
                        {
                            targetUser.BringVoiceBack(session.User);
                        }
                        response.Result = DevoiceResponseType.Success;
                        uow.Commit();
                    }
                    catch (ModeratorsRightsRequiredException)
                    {
                        response.Result = DevoiceResponseType.Failed;
                    }
                }
            }
            
            if (response.Result == DevoiceResponseType.Success && targetUser != null)
            {
                //send a notification to everybody that we've devoiced him\her
                _sessionManager.SendToEachChatSessions(
                    new DevoiceNotification
                    {
                        ActorName = session.User.Name, 
                        Reason = request.Reason,
                        Devoice = request.Devoice,
                        TargetId = session.User.Id,
                        TargetName = targetUser.Name
                    });
            }
            return response;
        }

        public BanResponse Ban(ISession session, BanRequest request)
        {
            var response = request.CreateResponse<BanResponse>();

            User targetUser = null;

            using (var uow = UnitOfWorkFactory.Create())
            {
                targetUser = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (targetUser != null)
                {
                    try
                    {
                        if (request.Ban)
                            targetUser.Ban(session.User);
                        else
                            targetUser.UnBan(session.User);

                        response.Result = BanResponseType.Success;
                        uow.Commit();
                    }
                    catch (ModeratorsRightsRequiredException)
                    {
                        response.Result = BanResponseType.Failed;
                    }
                }
            }

            if (response.Result == BanResponseType.Success && targetUser != null)
            {
                //let's kick him\her from the server!
                _sessionManager.CloseSessionByUserId(targetUser.Id);

                //send a notification to everybody that we've banned him\her
                _sessionManager.SendToEachChatSessions(
                    new BanNotification
                    {
                        Ban = request.Ban,
                        ActorName = session.User.Name,
                        Reason = request.Reason,
                        TargetId = session.User.Id,
                        TargetName = targetUser.Name
                    });
            }

            return response;
        }

        /// <summary>
        /// Resets user's photo to default one
        /// </summary>
        public ResetPhotoResponse ResetPhoto(ISession session, ResetPhotoRequest request)
        {
            var response = request.CreateResponse<ResetPhotoResponse>();
            try
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    var target = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetId));
                    if (target != null)
                    {
                        response.NewPhotoId = target.ResetPhoto(session.User);
                        uow.Commit();
                        response.Success = true;
                    }
                }
            }
            catch (ModeratorsRightsRequiredException)
            {
                response.Success = false;
            }
            if (response.Success)
            {
                _profileChangesNotificator.NotifyEverybodyInChatAboutProfileChanges(request.TargetId, new Dictionary<string,object> {{ "PhotoId", response.NewPhotoId }});
            }
            return response;
        }

        public LastMessageResponse GetLastMessages(ISession session, LastMessageRequest request)
        {
            var response = request.CreateResponse<LastMessageResponse>(); 

            List<PublicMessageDto> messages = null;
            using (var uow = UnitOfWorkFactory.Create())
            {
                messages = uow.PublicMessageRepository
                    .TakeLast(_settings.LastMessagesCount)
                    .ProjectedAsCollection<PublicMessageDto>()
                    .ToList();
            }
            response.Subject = _settings.Subject;
            response.Messages = messages.ToArray();
            return response;
        }

        public GetOnlineUsersResponse GetOnlineUsers(ISession session, GetOnlineUsersRequest request)
        {
            var response = request.CreateResponse<GetOnlineUsersResponse>(); 

            var players = new List<UserDto>(); 
            foreach (var onlineSession in _sessionManager.GetActiveSessions())
            {
                var playerDto = onlineSession.Value.User.ProjectedAs<UserDto>();
                players.Add(playerDto);
            }

            response.Users = players.ToArray();
            return response;
        }

        private void SessionManagerOnAuthenticatedUserDisconnected(object sender, SessionEventArgs e)
        {
            //send notification to everyone that someone has left
            _sessionManager.SendToEachChatSessionsExcept(
                new LeftUserInfo { UserId = e.Session.User.Id }, e.Session.User.Id);
        }

        private void SessionManagerOnAuthenticatedUserConnected(object sender, SessionEventArgs e)
        {
            //send notification to everyone that someone has joined
            _sessionManager.SendToEachChatSessionsExcept(
                new JoinedUserInfo { User = e.Session.User.ProjectedAs<UserDto>() }, e.Session.User.Id);
        }
    }
}

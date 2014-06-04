using System.Linq;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Application.Services
{
    public class FriendsService : AppService
    {
        public FriendsService(IUnitOfWorkFactory unitOfWorkFactory)
            : base(unitOfWorkFactory)
        {
        }

        public UserFriendsResponse GetFriends(ISession session, UserFriendsRequest request)
        {
            var response = request.CreateResponse<UserFriendsResponse>();
            response.Friends = session.User.Friends.ProjectedAsCollection<UserDto>().ToArray();
            return response;
        }

        public AddToFriendsResponse AddToFriends(ISession session, AddToFriendsRequest request)
        {
            var response = request.CreateResponse<AddToFriendsResponse>();
            response.Success = true;

            if (session.User.Friends.Any(i => i.Id == request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var friend = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (friend == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.Friends.Add(friend);
                    uow.Commit();
                }
            } 
            return response;
        }

        public RemoveFromFriendsResponse RemoveFromFriends(ISession session, RemoveFromFriendsRequest request)
        {
            var response = request.CreateResponse<RemoveFromFriendsResponse>();
            response.Success = true;
            
            if (session.User.Friends.All(i => i.Id != request.TargetUserId))
            {
                response.Success = false;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                var friend = uow.UsersRepository.FirstMatching(UserSpecification.Id(request.TargetUserId));
                if (friend == null)
                {
                    response.Success = false;
                }
                else
                {
                    session.User.Friends.Remove(friend);
                    uow.Commit();
                }
            } 
            return response;
        }

    }
}

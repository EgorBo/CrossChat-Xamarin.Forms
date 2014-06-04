using System.Linq;
using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Application.Services
{
    public class AuthenticationService : AppService
    {
        private readonly ISettings _settings;

        public AuthenticationService(
            ISettings settings,
            IUnitOfWorkFactory unitOfWorkFactory)
            : base(unitOfWorkFactory)
        {
            _settings = settings;
        }

        public AuthenticationResponse Authenticate(ISession session, AuthenticationRequest request)
        {
            var response = request.CreateResponse<AuthenticationResponse>();
            
            response.Result = AuthenticationResponseType.Success;

            using (var uow = UnitOfWorkFactory.Create())
            {
                var user = uow.UsersRepository.FirstMatching(UserSpecification.NameAndPassword(request.Name, request.Password));
                if (user == null)
                {
                    response.Result = AuthenticationResponseType.InvalidNameOrPassword;
                    return response;
                }
                else
                {
                    if (user.IsBanned)
                    {
                    }
                    else if (user.Huid != request.Huid)
                    {
                        user.ChangeHuid(request.Huid);
                    }
                }
                
                uow.Commit();
                
                if (response.Result == AuthenticationResponseType.Success)
                {
                    Enumerable.Count(user.Friends);
                    session.SetUser(user);
                    response.User = user.ProjectedAs<UserDto>();
                }
            }
            return response;
        }
    }
}

using System.Linq;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Exceptions;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Application.Services
{
    /// <summary>
    /// </summary>
    public class RegistrationService : AppService
    {
        public RegistrationService(
            IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory)
        {
        }

        /// <summary>
        /// </summary>
        public RegistrationResponse RegisterNewUser(ISession session, RegistrationRequest request)
        {
            var response = request.CreateResponse(new RegistrationResponse { Result = RegistrationResponseType.Success });
            User user = null;
            
            try
            {
                user = new User(request.Name, request.Password, request.Huid, request.Sex, request.Age, request.PushUri, request.Country, request.Platform);
            }
            catch (InvalidUserRegistrationDataException)
            {
                response.Result = RegistrationResponseType.InvalidData;
                return response;
            }

            using (var uow = UnitOfWorkFactory.Create())
            {
                if (uow.UsersRepository.AnyMatching(UserSpecification.Name(user.Name)))
                {
                    response.Result = RegistrationResponseType.NameIsInUse;
                    return response;
                }

                uow.UsersRepository.Add(user);
                uow.Commit();
            }
            response.User = user.ProjectedAs<UserDto>();
            session.SetUser(user);
            return response;
        }

        /// <summary>
        /// </summary>
        public ResponseBase Deactivate(ISession session, DeactivationRequest request)
        {
            var response = request.CreateResponse();
            using (var uow = UnitOfWorkFactory.Create())
            {
                uow.Attach(session.User);
                session.User.Delete();
                uow.Commit();
            }
            return response;
        }
    }
}

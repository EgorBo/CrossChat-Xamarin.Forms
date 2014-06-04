using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Infrastructure.Persistence.EF
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISettings _settings;

        public UnitOfWorkFactory(ISettings settings)
        {
            _settings = settings;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork();
        }
    }
}
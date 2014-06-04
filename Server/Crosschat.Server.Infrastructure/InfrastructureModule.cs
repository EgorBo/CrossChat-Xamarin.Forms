using Autofac;
using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Domain.Seedwork;
using Crosschat.Server.Infrastructure.FileSystem;
using Crosschat.Server.Infrastructure.Logging;
using Crosschat.Server.Infrastructure.Persistence.EF;
using Crosschat.Server.Infrastructure.Protocol;
using Crosschat.Server.Infrastructure.Serialization;
using Crosschat.Server.Infrastructure.Transport;
using Crosschat.Utils.Logging;

namespace Crosschat.Server.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            LogFactory.Initialize(typeName => new ConsoleLogger(typeName));

            builder.RegisterType<InfrastructureInitializator>().As<IInfrastructureInitializator>().SingleInstance();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>().SingleInstance();
            builder.RegisterType<CrosschatSocketServer>().AsSelf().As<ISessionManager>().SingleInstance();
            builder.RegisterType<HardcodedSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<FileStorage>().As<IFileStorage>().SingleInstance();
            builder.RegisterType<ProtobufDtoSerializer>().As<IDtoSerializer>().SingleInstance();
            builder.RegisterType<CommandParser>().AsSelf().SingleInstance();
            builder.RegisterType<TransportPerformanceMeasurer>().AsSelf().SingleInstance();
            
            base.Load(builder);
        }
    }
}

using System;
using Autofac;
using Crosschat.Server.Application;
using Crosschat.Server.Application.Agents;
using Crosschat.Server.Application.Contracts;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Infrastructure;

namespace Crosschat.Server.ConsoleHost
{
    public static class Bootstrapper
    {
        private static bool _isRunning = false;

        public static IContainer Run()
        {
            if (_isRunning)
                throw new InvalidOperationException();
            _isRunning = true;

            var builder = new ContainerBuilder();
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<InfrastructureModule>();

            var container = builder.Build();
            ServiceLocator.Init(container);
            container.Resolve<IInfrastructureInitializator>().Init();
            container.Resolve<AgentManager>().Run();

            return container;
        }
    }
}

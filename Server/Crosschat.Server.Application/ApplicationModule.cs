using System.Reflection;
using Autofac;
using Crosschat.Server.Application.Agents;
using Crosschat.Server.Application.DataTransferObjects.Utils;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Services.Helpers;
using Module = Autofac.Module;

namespace Crosschat.Server.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestsHandler>().AsSelf().SingleInstance();
            builder.RegisterType<AgentManager>().AsSelf().SingleInstance();
            builder.RegisterType<ProfileChangesNotificator>().AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(i => i.IsSubclassOf(typeof(AppService))).AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(i => i.IsSubclassOf(typeof(ScheduledAgentBase))).AsSelf().SingleInstance();

            base.Load(builder);
        }
    }
}

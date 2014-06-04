using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Application.Services;
using Crosschat.Server.Application.Sessions;
using Crosschat.Server.Infrastructure.Protocol;
using Crosschat.Utils.Logging;

namespace Crosschat.Server.Infrastructure.Transport.Commands
{
    public class RequestCommand : CrosschatCommandBase
    {
        protected new readonly ILogger Logger = LogFactory.GetLogger<RequestCommand>();

        protected override bool AllowAnonymousAccess { get { return true; } }

        public override async void ExecuteAstralCommand(CrosschatSession session, byte[] data)
        {
            var request = DtoSerializer.Deserialize<RequestBase>(data);
            var methodAndInstancePair = AppServicesMethods[request.GetType()];

            Logger.Trace("{0}  {1}  {2}", methodAndInstancePair.Item1.Name, request.GetType().Name, session.GetSessionName());

            try
            {
                var responseObj = methodAndInstancePair.Item1.Invoke(methodAndInstancePair.Item2,
                    methodAndInstancePair.Item3 == ArgumentsOrder.SessionRequest ?
                    new object[] { session, request } :
                    new object[] { request, session });


                if (methodAndInstancePair.Item1.ReturnType.IsSubclassOf(typeof(Task)))
                {
                    var value = await (dynamic)responseObj;
                    session.Send(value);
                }
                else if (methodAndInstancePair.Item1.ReturnType != typeof(void))
                {
                    session.Send(responseObj);
                }
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "{0} failed ({1},  {2})", methodAndInstancePair.Item1.Name, request.GetType().Name, session.GetSessionName());
            }
        }

        protected override CommandNames CommandName
        {
            get { return CommandNames.Request; }
        }

        private static readonly Dictionary<Type, Tuple<MethodInfo, AppService, ArgumentsOrder>> AppServicesMethods =
            new Dictionary<Type, Tuple<MethodInfo, AppService, ArgumentsOrder>>();

        static RequestCommand()
        {
            try
            {
                var servicesTypes = typeof(ChatService).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(AppService)));
                var servicesInstances = new List<AppService>();
                var methods = new List<Tuple<MethodInfo, AppService>>();

                foreach (var serviceType in servicesTypes)
                {
                    var controller = ServiceLocator.Resolve(serviceType) as AppService;
                    servicesInstances.Add(controller);
                    methods.AddRange(serviceType.GetMethods().Select(i => new Tuple<MethodInfo, AppService>(i, controller)));
                }

                foreach (var method in methods)
                {
                    var parameters = method.Item1.GetParameters();
                    if (parameters.Length != 2)
                        continue;
                    if (parameters[0].ParameterType == typeof(ISession) && parameters[1].ParameterType.IsSubclassOf(typeof(RequestBase)))
                    {
                        AppServicesMethods[parameters[1].ParameterType] = new Tuple<MethodInfo, AppService, ArgumentsOrder>(method.Item1, method.Item2, ArgumentsOrder.SessionRequest);
                    }
                    else if (parameters[1].ParameterType == typeof(ISession) && parameters[0].ParameterType.IsSubclassOf(typeof(RequestBase)))
                    {
                        AppServicesMethods[parameters[0].ParameterType] = new Tuple<MethodInfo, AppService, ArgumentsOrder>(method.Item1, method.Item2, ArgumentsOrder.RequestSession);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw;
            }
        }

        enum ArgumentsOrder
        {
            SessionRequest,
            RequestSession
        }
    }
}

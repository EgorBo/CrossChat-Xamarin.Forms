using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Seedwork.Specifications;
using Crosschat.Server.Infrastructure.Persistence.EF;
using Crosschat.Server.Infrastructure.Transport;
using Crosschat.Utils.Logging;

namespace Crosschat.Server.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Bootstrapper.Run();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_OnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_OnUnobservedTaskException;
            Console.WriteLine("Bootstrapper is ran.");
            string command = "";

            while (command != "exit")
            {
                if (command == "stat") //show statistics
                {
                    var measurer = container.Resolve<TransportPerformanceMeasurer>();
                    Console.WriteLine("Sent: {0} ({2} commands),  Received: {1} ({3} commands). MaxCommandSize: {4}, TotalConnections: {5}",
                        StringExtensions.ToShortSizeInBytesString(measurer.SentBytes),
                        StringExtensions.ToShortSizeInBytesString(measurer.ReceivedBytes),
                        measurer.SentCommands, measurer.ReceivedCommands, StringExtensions.ToShortSizeInBytesString(measurer.MaxCommandSize), measurer.TotalConnections);
                }
                command = Console.ReadLine();
            }
        }

        private static void TaskScheduler_OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogFactory.GetLogger<Program>().Exception(e.Exception);
        }

        private static void CurrentDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogFactory.GetLogger<Program>().Exception(e.ExceptionObject as Exception);
        }
    }
}

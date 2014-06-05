using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using Crosschat.Server.Application.Seedwork;
using Crosschat.Utils.Logging;

namespace Crosschat.Server.Application.Agents
{
    public class AgentManager
    {
        protected static readonly ILogger Logger = LogFactory.GetLogger<AgentManager>();
        private Dictionary<ScheduledAgentBase, DateTime> _jobs = new Dictionary<ScheduledAgentBase, DateTime>();
        private static readonly object SyncObj = new object();
        private Timer _timer = null;

        public void Run()
        {
            if (_timer != null)
                throw new InvalidOperationException("AgentManager is already runned");

            _jobs = Assembly.GetExecutingAssembly().GetTypes()
                .Where(i => i.BaseType == typeof(ScheduledAgentBase))
                .Select(i => ServiceLocator.Resolve(i) as ScheduledAgentBase)
                .ToDictionary(i => i, i => DateTime.Now);

            _jobs.Where(i => !i.Key.DontRunOnStart).ForEach(i => i.Key.Execute());
            Logger.Warning("Run() completed");

            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }
        
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var jobPair in _jobs.ToDictionary(i => i.Key, i => i.Value))
            {
                lock (SyncObj)
                {
                    DateTime date = jobPair.Value;
                    var job = jobPair.Key;
                    DateTime nowDate = DateTime.Now;
                    if (nowDate - date >= job.TimeSpan)
                    {
                        Logger.Debug("Executing agent " + job.GetType().Name);
                        _jobs[job] = DateTime.Now;
                        job.Execute();
                        _jobs[job] = DateTime.Now;
                    }
                }
            }
        }
    }
}

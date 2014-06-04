using System;
using System.Linq;
using AutoMapper;

namespace Crosschat.Server.Application.Seedwork
{
    public class AutomapperTypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        static AutomapperTypeAdapterFactory()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .SelectMany(a => a.GetTypes())
                                    .Where(t => t.BaseType == typeof(Profile));

            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.SelfProfiler`2")
                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                }
            });
        }

        #endregion

        #region ITypeAdapterFactory Members

        public static AutomapperTypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}

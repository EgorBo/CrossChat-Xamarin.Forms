using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Autofac;

namespace Crosschat.Server.Application.Seedwork
{
    public static class ServiceLocator
    {
        private static readonly ConcurrentDictionary<object[], WeakReference> Cache = new ConcurrentDictionary<object[], WeakReference>();
        private static ILifetimeScope _currentLifetimeScope = null;

        public static void Init(IContainer container)
        {
            Instance = container;
            TryDisposeLifetimeScope();
            _currentLifetimeScope = container.BeginLifetimeScope();
        }

        private static IContainer Instance { get; set; }

        [DebuggerStepThrough]
        public static T Resolve<T>()
        {
            return _currentLifetimeScope.Resolve<T>();
        }
        [DebuggerStepThrough]
        public static object Resolve(Type type)
        {
            return _currentLifetimeScope.Resolve(type);
        }

        public static void BeginLifetimeScope()
        {
            TryDisposeLifetimeScope();
            _currentLifetimeScope = Instance.BeginLifetimeScope();
        }

        [DebuggerStepThrough]
        public static T ResolveWith<T>(params object[] parameters) where T : class
        {
            return _currentLifetimeScope.ResolveOptional<T>(parameters.Select(i => new TypedParameter(i.GetType(), i)));
        }

        private static void TryDisposeLifetimeScope()
        {
            if (_currentLifetimeScope != null)
            {
                _currentLifetimeScope.Dispose();
                _currentLifetimeScope = null;
            }
        }

        [DebuggerStepThrough]
        public static T ResolveWithCache<T>(params object[] parameters) where T : class
        {
            foreach (var cacheItem in Cache.ToList())
            {
                if (cacheItem.Key.SequenceEqual(parameters) && cacheItem.Value.IsAlive)
                {
                    return (T)cacheItem.Value.Target;
                }
            }
            var instance = ResolveWith<T>(parameters);
            Cache[parameters] = new WeakReference(instance);
            return instance;
        }
    }
}

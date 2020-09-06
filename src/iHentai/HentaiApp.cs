using System;
using System.Collections.Concurrent;
using System.Net.Http;
using iHentai.Common;
using iHentai.Data;
using iHentai.Extensions;
using iHentai.Extensions.Runtime;
using iHentai.Platform;

namespace iHentai
{
    internal static class Ioc
    {
        private static readonly ConcurrentDictionary<Type, object>
            _container = new ConcurrentDictionary<Type, object>();

        public static void Register<T, V>(this object _, Func<V> generator) where V : T
        {
            _container.TryAdd(typeof(T), generator.Invoke());
        }

        public static void Register<T, V>(this object _) where V : T, new()
        {
            _container.TryAdd(typeof(T), new V());
        }

        public static T Resolve<T>(this object _)
        {
            if (_container.TryGetValue(typeof(T), out var result))
            {
                return (T) result;
            }

            return default;
        }
    }

    internal class HentaiApp
    {
        public static HentaiApp Instance { get; } = new HentaiApp();

        public void Init()
        {
            this.Register<HttpMessageHandler, HentaiHttpHandler>(() => HentaiHttpHandler.Instance);
            this.Register<IExtensionStorage, ExtensionStorage>();
            this.Register<IPlatformService, PlatformService>();
            this.Register<IExtensionManager, ExtensionManager>();
        }
    }
}
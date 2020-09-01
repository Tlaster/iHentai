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
    public class HentaiApp
    {
        private readonly ConcurrentDictionary<Type, object> _container = new ConcurrentDictionary<Type, object>();
        
        private HentaiApp()
        {
            Register<HttpMessageHandler, HentaiHttpHandler>();
            Register<IExtensionStorage, ExtensionDb>(() => ExtensionDb.Instance);
            Register<IPlatformService, PlatformService>();
        }

        public static HentaiApp Instance { get; } = new HentaiApp();
        public ExtensionManager ExtensionManager { get; } = new ExtensionManager();


        public void Register<T, V>(Func<V> generator) where V : T
        {
            _container.TryAdd(typeof(T), generator.Invoke());
        }

        public void Register<T, V>() where V: T, new()
        {
            _container.TryAdd(typeof(T), new V());
        }

        public T Resolve<T>()
        {
            if (_container.TryGetValue(typeof(T), out var result))
            {
                return (T)result;
            } else
            {
                return default;
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using iHentai.Services.Core;

namespace iHentai.Services
{
    public static class iHentaiService
    {
        private static readonly ConcurrentDictionary<Type, object> _instances = new ConcurrentDictionary<Type, object>();

        public static void Register<T>(object instance)
        {
            _instances.TryAdd(typeof(T), instance);
        }

        internal static T Resolve<T>(this IApi api)
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T) instance;
            }

            return default;
        }
    }
}
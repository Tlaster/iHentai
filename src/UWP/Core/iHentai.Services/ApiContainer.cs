using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Windows.UI.Xaml;
using iHentai.Basic.Helpers;

namespace iHentai.Services
{
    public interface IApiApplication
    {
        IEnumerable<Assembly> GetApiAssemblies();
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ApiKeyAttribute : Attribute
    {
        public ApiKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }

    public interface IApi
    {
    }

    public interface IWithHttpHandler
    {
        bool CanHandle(HttpRequestMessage message);
        void Handle(ref HttpRequestMessage message);
    }

    public class ApiContainer
    {
        public ApiContainer()
        {
            if (Application.Current is IApiApplication application)
                KnownApis = application
                    .GetApiAssemblies()
                    .SelectMany(item => item.DefinedTypes)
                    .Where(x => x.IsClass && x.ImplementedInterfaces.Contains(typeof(IApi)) &&
                                x.GetCustomAttribute<ApiKeyAttribute>() != null)
                    .Select(x => x)
                    .ToDictionary(x => x.GetCustomAttribute<ApiKeyAttribute>().Key, x => x);
            else
                throw new NotImplementedException();
        }

        public Dictionary<string, TypeInfo> KnownApis { get; }

        public IApi this[Enum index] => this[Enum.GetName(index.GetType(), index)];

        public IApi this[string index] => Apis.GetOrAdd(index, str => (IApi) Activator.CreateInstance(KnownApis[str]));

        public ConcurrentDictionary<string, IApi> Apis { get; } = new ConcurrentDictionary<string, IApi>();

        public void HandleHttpMessage(ref HttpRequestMessage message)
        {
            var copy = message;
            (Apis.Values?.FirstOrDefault(item =>
                    item is IWithHttpHandler httpHandler &&
                    httpHandler.CanHandle(copy)) as IWithHttpHandler)?
                .Handle(ref message);
        }
    }

    public static class ApiExtensions
    {
        public static T Get<T>(this Enum @enum) where T : IApi
        {
            return (T) Singleton<ApiContainer>.Instance[@enum];
        }

        public static T Get<T>(this string value) where T : IApi
        {
            return (T) Singleton<ApiContainer>.Instance[value];
        }
    }
}
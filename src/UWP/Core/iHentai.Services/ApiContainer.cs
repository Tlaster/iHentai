using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace iHentai.Services
{
    public class ApiContainer
    {
        public ApiContainer()
        {
            if (Application.Current is IApiApplication application)
            {
                KnownApis = application
                    .GetApiAssemblies()
                    .SelectMany(item => item.DefinedTypes)
                    .Where(x => x.IsClass && !x.IsAbstract && x.ImplementedInterfaces.Contains(typeof(IApi)) &&
                                x.GetCustomAttribute<ApiKeyAttribute>() != null)
                    .ToDictionary(x => x.GetCustomAttribute<ApiKeyAttribute>().Key, x => x);

                ApiEntries = application.GetEntries().ToDictionary(x => x.ApiAssembly,
                    x => x.ViewAssembly.DefinedTypes.FirstOrDefault(item =>
                        item.GetCustomAttribute<StartupAttribute>() != null));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<Assembly, TypeInfo> ApiEntries { get; }

        public Dictionary<string, TypeInfo> KnownApis { get; }

        public IInstanceData CurrentInstanceData { get; set; }

        public IApi this[string index] => Apis.GetOrAdd(index, str => (IApi) Activator.CreateInstance(KnownApis[str]));

        public ConcurrentDictionary<string, IApi> Apis { get; } = new ConcurrentDictionary<string, IApi>();

        public Type Navigation(string service)
        {
            return ApiEntries.TryGetValue(this[service].GetType().Assembly, out var value) ? value : null;
        }
    }
}
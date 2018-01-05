using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                    .Where(x => x.IsClass && x.ImplementedInterfaces.Contains(typeof(IApi)) &&
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
        
        public ConcurrentDictionary<Guid, IInstanceData> InstanceDatas { get; } = new ConcurrentDictionary<Guid, IInstanceData>();

        public Dictionary<Assembly, TypeInfo> ApiEntries { get; }

        public Dictionary<string, TypeInfo> KnownApis { get; }

        public IInstanceData this[Guid index]
        {
            get => InstanceDatas.TryGetValue(index, out var value) ? value : null;
            set => InstanceDatas.TryAdd(index, value);
        }
        
        public IApi this[string index] => Apis.GetOrAdd(index, str => (IApi) Activator.CreateInstance(KnownApis[str]));

        public ConcurrentDictionary<string, IApi> Apis { get; } = new ConcurrentDictionary<string, IApi>();

        public Type Navigation(string service)
        {
            return ApiEntries.TryGetValue(this[service].GetType().Assembly, out var value) ? value : null;
        }
    }
}
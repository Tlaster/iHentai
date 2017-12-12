using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iHentai.Apis.Core
{
    public class HentaiServices
    {
        private HentaiServices()
        {
            KnownApis = typeof(IHentaiApis).GetTypeInfo().Assembly.DefinedTypes.Where(item =>
                item.IsClass && item.ImplementedInterfaces.Contains(typeof(IHentaiApis)));
        }

        public IEnumerable<TypeInfo> KnownApis { get; }

        private ConcurrentDictionary<ServiceTypes, IHentaiApis> Services { get; } = new ConcurrentDictionary<ServiceTypes, IHentaiApis>();

        public static HentaiServices Instance { get; } = new HentaiServices();

        public IHentaiApis this[string host] => Services.FirstOrDefault(item => item.Value.Host == host).Value;

        public IHentaiApis this[ServiceTypes type]
        {
            get
            {
                if (!Services.ContainsKey(type))
                    Services.TryAdd(type,
                        Activator.CreateInstance(
                            KnownApis.FirstOrDefault(item =>
                                item.Namespace.Contains(Enum.GetName(typeof(ServiceTypes), type)))) as IHentaiApis);
                return Services[type];
            }
        }
    }
}
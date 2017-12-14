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
            KnownApis = typeof(IHentaiApi).GetTypeInfo().Assembly.DefinedTypes.Where(item =>
                item.IsClass && item.ImplementedInterfaces.Contains(typeof(IHentaiApi)));
        }

        public IEnumerable<TypeInfo> KnownApis { get; }

        private ConcurrentDictionary<ServiceTypes, IHentaiApi> Services { get; } = new ConcurrentDictionary<ServiceTypes, IHentaiApi>();

        public static HentaiServices Instance { get; } = new HentaiServices();

        public IHentaiApi this[string host] => Services.FirstOrDefault(item => item.Value.Host == host).Value;

        public IHentaiApi this[ServiceTypes type]
        {
            get
            {
                if (!Services.ContainsKey(type))
                    Services.TryAdd(type,
                        Activator.CreateInstance(
                            KnownApis.FirstOrDefault(item =>
                                item.Namespace.Contains(Enum.GetName(typeof(ServiceTypes), type)))) as IHentaiApi);
                return Services[type];
            }
        }
    }
}
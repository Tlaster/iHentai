using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iHentai.Apis.Core
{
    public enum ServiceTypes
    {
        NHentai,
        EHentai
    }

    public class ServiceInstances
    {
        private ServiceInstances()
        {
            KnownApis = typeof(IHentaiApis).GetTypeInfo().Assembly.DefinedTypes.Where(item =>
                item.IsClass && item.ImplementedInterfaces.Contains(typeof(IHentaiApis)));
        }

        public IEnumerable<TypeInfo> KnownApis { get; }

        private Dictionary<ServiceTypes, IHentaiApis> Services { get; } = new Dictionary<ServiceTypes, IHentaiApis>();

        public static ServiceInstances Instance { get; } = new ServiceInstances();

        public IHentaiApis this[string host] => Services.FirstOrDefault(item => item.Value.Host == host).Value;

        public IHentaiApis this[ServiceTypes type]
        {
            get
            {
                if (!Services.ContainsKey(type))
                    Services.Add(type,
                        Activator.CreateInstance(
                            KnownApis.FirstOrDefault(item =>
                                item.Namespace.Contains(Enum.GetName(typeof(ServiceTypes), type)))) as IHentaiApis);
                return Services[type];
            }
        }
    }
}
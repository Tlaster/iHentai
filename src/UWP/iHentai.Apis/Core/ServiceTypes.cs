using System;
using System.Collections.Generic;
using System.Linq;

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
//            Services = Enum.GetValues(typeof(ServiceTypes)).Cast<ServiceTypes>().ToDictionary(item => item,
//                item => Activator.CreateInstance(
//                    Type.GetType($"iHentai.Services.{Enum.GetName(typeof(ServiceTypes), item)}.Apis")) as IHentaiApis);
        }

        private Dictionary<ServiceTypes, IHentaiApis> Services { get; } = new Dictionary<ServiceTypes, IHentaiApis>();

        public static ServiceInstances Instance { get; } = new ServiceInstances();

        public IHentaiApis this[ServiceTypes type]
        {
            get
            {
                if (!Services.ContainsKey(type))
                    Services.Add(type,
                        Activator.CreateInstance(
                                Type.GetType($"iHentai.Apis.{Enum.GetName(typeof(ServiceTypes), type)}.Apis")) as
                            IHentaiApis);
                return Services[type];
            }
        }

        public IHentaiApis this[string host] => Services.FirstOrDefault(item => item.Value.Host == host).Value;
    }
}
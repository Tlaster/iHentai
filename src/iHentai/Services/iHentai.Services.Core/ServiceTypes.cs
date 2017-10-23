using System;
using System.Collections.Generic;
using System.Linq;

namespace iHentai.Services.Core
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
            Services = Enum.GetValues(typeof(ServiceTypes)).Cast<ServiceTypes>().ToDictionary(item => item,
                item => Activator.CreateInstance(
                    Type.GetType($"iHentai.Services.{Enum.GetName(typeof(ServiceTypes), item)}.Apis")) as IHentaiApis);
        }

        private Dictionary<ServiceTypes, IHentaiApis> Services { get; }

        public static ServiceInstances Instance { get; } = new ServiceInstances();

        public IHentaiApis this[ServiceTypes type] => Services[type];
    }
}
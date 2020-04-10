using System;
using System.Collections.Generic;
using System.Linq;
using iHentai.Activities.EHentai;
using iHentai.Activities.Generic;
using iHentai.Services.Core;
using iHentai.Services.EHentai;
using iHentai.Services.Manhuagui;
using Microsoft.Toolkit.Helpers;

namespace iHentai.Services
{
    internal interface IServiceModel
    {
        string Name { get; }
        Type StartActivity { get; }
        Dictionary<string, object> Intent { get; }
    }

    internal class ServiceModel<T> : IServiceModel
    {
        public ServiceModel(string name, Dictionary<string, object> intent = null)
        {
            Name = name;
            Intent = intent;
        }

        public string Name { get; }
        public Type StartActivity { get; } = typeof(T);
        public Dictionary<string, object> Intent { get; }
    }

    internal class MangaServiceModel<TApi> : ServiceModel<MangaGalleryActivity>
        where TApi : IMangaApi, new()
    {
        public MangaServiceModel() : base(Singleton<TApi>.Instance.Name,
            new Dictionary<string, object> {{"api", Singleton<TApi>.Instance}})
        {
        }
    }
    internal class MangaServiceModel : ServiceModel<MangaGalleryActivity>
    {
        private IMangaApi _instance;

        public MangaServiceModel(IMangaApi instance) : base(instance.Name,
            new Dictionary<string, object> {{"api", instance}})
        {
            _instance = instance;
        }
    }

    internal class ExHentaiServiceModel : IServiceModel
    {
        public string Name { get; } = "ExHentai";

        public Type StartActivity
        {
            get
            {
                if (Singleton<ExApi>.Instance.RequireLogin)
                {
                    return typeof(LoginActivity);
                }

                return typeof(GalleryActivity);
            }
        }

        public Dictionary<string, object> Intent => new Dictionary<string, object>
        {
            {"api", Singleton<ExApi>.Instance}
        };
    }

    internal class ServiceManager
    {
        public ServiceManager()
        {
            Services.Add(new ServiceModel<GalleryActivity>("E-Hentai"));
            Services.Add(new ExHentaiServiceModel());

            var apis = typeof(IMangaApi).Assembly.DefinedTypes.Where(it =>
                typeof(IMangaApi).IsAssignableFrom(it) && it.IsClass && !it.IsAbstract);
            foreach (var item in apis)
            {
                if (Activator.CreateInstance(item) is IMangaApi api)
                {
                    Services.Add(new MangaServiceModel(api));
                }
            }
        }

        public List<IServiceModel> Services { get; } = new List<IServiceModel>();
    }
}
using System;
using System.Collections.Generic;
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
            Services.Add(new MangaServiceModel<ManhuaguiApi>());
        }

        public List<IServiceModel> Services { get; } = new List<IServiceModel>();
    }
}
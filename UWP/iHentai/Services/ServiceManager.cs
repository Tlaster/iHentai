using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Activities.EHentai;
using iHentai.Activities.Manhuagui;
using iHentai.Services.EHentai;
using Microsoft.Toolkit.Helpers;

namespace iHentai.Services
{
    interface IServiceModel
    {
        string Name { get; }
        Type StartActivity { get; }
        Dictionary<string, object> Intent { get; }
    }

    class ServiceModel<T> : IServiceModel
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

    class ExHentaiServiceModel : IServiceModel
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
                else
                {
                    return typeof(GalleryActivity);
                }
            }
        }

        public Dictionary<string, object> Intent => new Dictionary<string, object>
        {
            {"api", Singleton<ExApi>.Instance}
        };
    }

    class ServiceManager
    {
        public List<IServiceModel> Services { get; } = new List<IServiceModel>();

        public ServiceManager()
        {
            Services.Add(new ServiceModel<GalleryActivity>("E-Hentai"));
            Services.Add(new ExHentaiServiceModel());
            Services.Add(new ServiceModel<ManhuaguiUpdateActivity>("Manhuagui"));
        }
    }
}

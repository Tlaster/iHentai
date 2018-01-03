using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Database;
using iHentai.Database.Models;
using iHentai.Mvvm;
using iHentai.Services;
using PropertyChanged;

namespace iHentai.ViewModels
{
    public class ServiceSelectionViewModel : ViewModel
    {
        public ServiceSelectionViewModel()
        {
            SelectedService = Source.FirstOrDefault();
            using (var context = new ApplicationDbContext())
            {
                Instances = context.Instances.ToList().GroupBy(item => item.Service, item => item.Data).ToList();
            }
        }

        public List<IGrouping<string, string>> Instances { get; }

        public List<ServiceSelectionBannerModel> Source { get; } =
            Singleton<ApiContainer>.Instance.KnownApis.Keys.Select(
                item => new ServiceSelectionBannerModel(item)).ToList();
        
        public ServiceSelectionBannerModel SelectedService { get; set; }

        public string Title { get; } = Package.Current.DisplayName;

        public ILoginData LoginData { get; set; }

        private void OnSelectedServiceChanged()
        {
            LoginData = SelectedService?.ServiceType?.Get<ICanLogin>()?.LoginDataGenerator;
        }

        public async void Confirm()
        {
            var service = SelectedService.ServiceType;
            var api = service.Get<IApi>();
            IInstanceData data = null;
            if (api is ICanLogin canLogin)
            {
                data = await canLogin.Login(LoginData);
            }
            Go(service, data);
        }

        private void Go(string service, IInstanceData data)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Instances.Add(new InstanceModel
                {
                    Service = service,
                    Data = data?.ToJson() ?? string.Empty,
                });
                context.SaveChanges();
            }
            Navigate(Singleton<ApiContainer>.Instance.Navigation(service), service, data).FireAndForget();
        }
    }
}
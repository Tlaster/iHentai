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
            using (var context = new ApplicationDbContext())
            {
                Instances = context.Instances.GroupBy(item => item.Service, item => item.Data)
                    .ToDictionary(item => item.Key, item => item.ToList());
            }

            SelectedService = Source.FirstOrDefault();
        }

        public bool IsLoading { get; set; }

        public Dictionary<string, List<string>> Instances { get; }

        public List<ServiceSelectionBannerModel> Source { get; } =
            Singleton<ApiContainer>.Instance.KnownApis.Keys.Select(
                item => new ServiceSelectionBannerModel(item)).ToList();

        [DependsOn(nameof(SelectedService))]
        public bool IsSingletonAccountApi
        {
            get
            {
                using (var context = new ApplicationDbContext())
                {
                    var instance =
                        context.Instances.FirstOrDefault(item => item.Service == SelectedService.ServiceType);
                    return SelectedService?.ServiceType?.Get<ISingletonLogin>() != null && instance != null;
                }
            }
        }

        public ServiceSelectionBannerModel SelectedService { get; set; }

        public string Title { get; } = Package.Current.DisplayName;

        public ILoginData LoginData { get; set; }

        public List<string> Datas { get; private set; }

        private void OnSelectedServiceChanged()
        {
            LoginData = SelectedService?.ServiceType?.Get<ICanLogin>()?.LoginDataGenerator;
            Datas = Instances.TryGetValue(SelectedService?.ServiceType, out var value) ? value : null;
        }

        public async void Confirm()
        {
            IsLoading = true;
            var service = SelectedService.ServiceType;
            var api = service.Get<IApi>();
            IInstanceData data = null;
            if (api is ICanLogin canLogin) data = await canLogin.Login(LoginData);
            Go(service, data);
            IsLoading = false;
        }

        public void ConfirmWithExistingAccount()
        {
            if (SelectedService?.ServiceType == null) return;
            IsLoading = true;

            using (var context = new ApplicationDbContext())
            {
                var instance = context.Instances.FirstOrDefault(item => item.Service == SelectedService.ServiceType);
                if (instance != null) Go(instance.Service, instance.Data.JsonToObject(instance.Service?.Get<ICanLogin>()?.InstanceDataType) as IInstanceData);
            }

            IsLoading = false;
        }

        public void GoSettings()
        {
            Navigate<SettingsViewModel>().FireAndForget();
        }

        private void Go(string service, IInstanceData data)
        {
            if (data != null && !Singleton<ApiContainer>.Instance.Contains(data))
            {
                using (var context = new ApplicationDbContext())
                {
                    var instance = context.Instances.FirstOrDefault(item => item.Service == service);
                    if (service.Get<ISingletonLogin>() != null && instance != null)
                    {
                        instance.Data = data.ToJson();
                        context.Instances.Update(instance);
                    }
                    else
                    {
                        context.Instances.Add(new InstanceModel
                        {
                            Service = service,
                            Data = data.ToJson()
                        });
                    }

                    context.SaveChanges();
                }
            }

            Guid guid;
            if (Singleton<ApiContainer>.Instance.Contains(data))
            {
                guid = Singleton<ApiContainer>.Instance.GetGuid(data);
            }
            else
            {
                guid = Guid.NewGuid();
                Singleton<ApiContainer>.Instance[guid] = data;
            }

            Navigate(Singleton<ApiContainer>.Instance.Navigation(service), service, guid).FireAndForget();
        }
    }
}
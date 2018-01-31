using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using iHentai.Basic;
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
                Instances = context.Instances.ToList().GroupBy(item => item.Service, item => (item.Data, item.Key))
                    .ToDictionary(item => item.Key, item => item.ToList());
            }

            SelectedService = Source.FirstOrDefault();
        }

        public Dictionary<string, List<(string Data, int Key)>> Instances { get; set; }

        public bool IsLoading { get; set; }
        
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

        public ICommand RemoveInstanceDataCommand => new RelayCommand<KeyValuePair<int, IInstanceData>>(item =>
        {
            using (var context = new ApplicationDbContext())
            {
                context.Instances.Remove(context.Instances.Find(item.Key));
                context.SaveChanges();
                Instances = context.Instances.ToList().GroupBy(x => x.Service, x => (x.Data, item.Key))
                    .ToDictionary(x => x.Key, x => x.ToList());
                OnSelectedServiceChanged();
            }
        });

        public ServiceSelectionBannerModel SelectedService { get; set; }

        public string Title { get; } = Package.Current.DisplayName;

        public ILoginData LoginData { get; set; }

        public Dictionary<int, IInstanceData> Datas { get; private set; }

        private void OnSelectedServiceChanged()
        {
            LoginData = SelectedService?.ServiceType?.Get<ICanLogin>()?.LoginDataGenerator;
            Datas = Instances.TryGetValue(SelectedService?.ServiceType, out var value)
                ? value.ToDictionary(item => item.Key, item =>
                    item.Data.JsonToObject(SelectedService?.ServiceType?.Get<ICanLogin>()
                        ?.InstanceDataType) as IInstanceData)
                : null;
        }

        public async void Confirm()
        {
            IsLoading = true;
            var service = SelectedService.ServiceType;
            var api = service.Get<IApi>();
            IInstanceData data = null;
            if (api is ICanLogin canLogin) data = await canLogin.Login(LoginData);
            CheckAndGo(service, data);
            IsLoading = false;
        }

        public void InstanceDataClicked(object sender, ItemClickEventArgs e)
        {
            Go(SelectedService?.ServiceType, ((KeyValuePair<int, IInstanceData>)e.ClickedItem).Value);
        }

        public void ConfirmWithExistingAccount()
        {
            if (SelectedService?.ServiceType == null) return;
            IsLoading = true;

            using (var context = new ApplicationDbContext())
            {
                var instance = context.Instances.FirstOrDefault(item => item.Service == SelectedService.ServiceType);
                if (instance != null)
                    CheckAndGo(instance.Service,
                        instance.Data.JsonToObject(instance.Service?.Get<ICanLogin>()?.InstanceDataType) as
                            IInstanceData);
            }

            IsLoading = false;
        }

        public void GoSettings()
        {
            Navigate<SettingsViewModel>().FireAndForget();
        }

        private void CheckAndGo(string service, IInstanceData data)
        {
            if (data != null && !Singleton<ApiContainer>.Instance.Contains(data))
                using (var context = new ApplicationDbContext())
                {
                    var instance = context.Instances.FirstOrDefault(item => item.Service == service);
                    if (service.Get<ISingletonLogin>() != null && instance != null)
                    {
                        instance.Data = data.ToJson();
                        context.Instances.Update(instance);
                    }
                    else if (context.Instances.Local.All(item =>
                        item.Service == service &&
                        !Equals(item.Data.JsonToObject(service.Get<ICanLogin>().InstanceDataType), data)))
                    {
                        context.Instances.Add(new InstanceModel
                        {
                            Service = service,
                            Data = data.ToJson()
                        });
                    }

                    context.SaveChanges();
                }

            Go(service, data);
        }

        private void Go(string service, IInstanceData data)
        {
            var guid = Guid.NewGuid();
            if (Singleton<ApiContainer>.Instance.Contains(data))
                guid = Singleton<ApiContainer>.Instance.GetGuid(data);
            else
                Singleton<ApiContainer>.Instance[guid] = data;

            Navigate(Singleton<ApiContainer>.Instance.Navigation(service), service, guid).FireAndForget();
        }
    }
}
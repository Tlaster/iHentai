using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
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
        }

        public List<ServiceSelectionBannerModel> Source { get; } =
            Singleton<ApiContainer>.Instance.KnownApis.Keys.Select(
                item => new ServiceSelectionBannerModel(item)).ToList();

        [AlsoNotifyFor(nameof(LoginData))]
        public ServiceSelectionBannerModel SelectedService { get; set; }

        public string Title { get; } = Package.Current.DisplayName;

        public ILoginData LoginData => SelectedService?.ServiceType?.Get<ICanLogin>()?.LoginDataGenerator;

        public async void Confirm()
        {
            var service = SelectedService.ServiceType;
            var api = service.Get<IApi>();
            if (api is ICanLogin canLogin) await canLogin.Login(LoginData);
            Navigate(Singleton<ApiContainer>.Instance.Navigation(service)).FireAndForget();
        }
    }
}
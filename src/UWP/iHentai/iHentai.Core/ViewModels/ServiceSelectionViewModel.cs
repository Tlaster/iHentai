using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using PropertyChanged;

namespace iHentai.Core.ViewModels
{
    public class ServiceSelectionViewModel : ViewModel
    {
        public ServiceSelectionViewModel()
        {
            LoginCommand = new RelayAsyncCommand(Login);
            SelectedService = Source.FirstOrDefault();
        }

        public List<ServiceSelectionBannerModel<ServiceTypes>> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel<ServiceTypes>(item)).ToList();

        public ServiceSelectionBannerModel<ServiceTypes> SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public IHentaiApi Api =>
            SelectedService?.ServiceType.Get<IHentaiApi>();

        [DependsOn(nameof(SelectedService))]
        public bool CanLogin => Api is ILoginApi;

        public string UserName { get; set; }

        public string Password { get; set; }

        public IAsyncCommand LoginCommand { get; }

        private async Task Login()
        {
            if (UserName.IsEmpty() || Password.IsEmpty() || !(Api is ILoginApi login))
                return;
            if (await login.Login(UserName, Password))
                OnSeccess();
        }

        public async void GoWebView()
        {
            if (await Navigate<LoginWebViewViewModel, bool>(args: Api))
            {
                OnSeccess();
                await (Api as ILoginApi).WebViewLoginFollowup();
            }
        }

        private void OnSeccess()
        {
            Skip();
        }

        public void Skip()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Navigate<GalleryViewModel>(args: SelectedService.ServiceType); //TODO: Navigate might failed if without WebViewLoginFollowup
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void Cancel()
        {
        }
    }
}
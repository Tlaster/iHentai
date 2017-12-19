using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Apis.Core;
using iHentai.Apis.Core.Models;
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
        
        public List<ServiceSelectionBannerModel> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel(item)).ToList();

        public ServiceSelectionBannerModel SelectedService { get; set; }

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
            {
                OnSeccess();
            }
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
            Navigate<GalleryViewModel>(args: SelectedService.ServiceType);//TODO: Navigate might failed if without WebViewLoginFollowup
            //Frame.ClearBackStack();
        }

        public void Cancel()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;
using PropertyChanged;

namespace Conet.ViewModels
{
    public class ApiSelectionViewModel : ViewModel
    {
        public ApiSelectionViewModel()
        {
            SelectedService = Source.FirstOrDefault();
            ConfirmCommand = new RelayAsyncCommand(Confirm);
        }

        public List<ServiceSelectionBannerModel<ServiceTypes>> Source { get; } = Enum.GetNames(typeof(ServiceTypes))
            .Select(item => new ServiceSelectionBannerModel<ServiceTypes>(item)).ToList();

        public ServiceSelectionBannerModel<ServiceTypes> SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public IConetApi Api =>
            SelectedService?.ServiceType.Get<IConetApi>();

        public ILoginData LoginData { get; private set; }

        public IAsyncCommand ConfirmCommand { get; }

        private async Task Confirm()
        {
            var oauth = await Api.GetOAuth(LoginData);
            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(oauth.Uri), new Uri(oauth.CallbackUri));
            if (result.ResponseStatus != WebAuthenticationStatus.Success) return;
            if (await Api.OAuthResponseHandler(result.ResponseData))
            {
                
            }
        }

        public void OnSelectedServiceChanged()
        {
            LoginData = SelectedService?.ServiceType.Get<IConetApi>()?.LoginDataGenerator;
        }
    }
}
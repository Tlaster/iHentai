using System;
using System.Windows.Input;
using Acr.UserDialogs;
using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;
using PropertyChanged;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(ServiceSelectionPage))]
    public class ServiceSelectionViewModel : ViewModel<IHentaiApis>
    {
        public ServiceSelectionViewModel()
        {
            SelectedService = Services[0];
        }

        public string[] Services { get; } = Enum.GetNames(typeof(ServiceTypes));
        public string SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public IHentaiApis SelectedApi =>
            ServiceInstances.Instance[(ServiceTypes) Enum.Parse(typeof(ServiceTypes), SelectedService)];

        public string UserName { get; set; }
        public string Password { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => { });

        public ICommand ConfirmCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return;
            UserDialogs.Instance.ShowLoading();
            var res = await SelectedApi.Login(UserName, Password);
            UserDialogs.Instance.HideLoading();
            if (res.State)
                Close(SelectedApi);
            else
                UserDialogs.Instance.Toast(res.Message);
        });

        public ICommand SkipCommand => new RelayCommand(() => { Close(SelectedApi); });
    }
}
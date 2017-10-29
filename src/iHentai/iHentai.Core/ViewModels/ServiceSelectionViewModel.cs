using System;
using System.Windows.Input;
using iHentai.Core.Common.Helpers;
using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;
using PropertyChanged;
using Xamarin.Forms;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(ServiceSelectionPage))]
    public class ServiceSelectionViewModel : ViewModel
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

        public ICommand ConfirmCommand => new RelayCommand(() => { });

        public ICommand SkipCommand => new RelayCommand(() =>
        {
            Application.Current.MainPage.Navigation.PushAsync(new MainPage(SelectedApi));
        });
    }
}
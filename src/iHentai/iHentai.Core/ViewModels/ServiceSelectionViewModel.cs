using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using iHentai.Services.Core;
using iHentai.Common.Helpers;
using iHentai.Core.Views;

namespace iHentai.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ServiceSelectionViewModel
    {
        public ServiceSelectionViewModel()
        {
            SelectedService = Services[0];
        }

        public string[] Services { get; } = Enum.GetNames(typeof(ServiceTypes));
        public string SelectedService { get; set; }

        [DependsOn(nameof(SelectedService))]
        public IHentaiApis SelectedApi => ServiceInstances.Instance[(ServiceTypes) Enum.Parse(typeof(ServiceTypes), SelectedService)];

        public string UserName { get; set; }
        public string Password { get; set; }

        public ICommand CancelCommand => new RelayCommand(() =>
        {
        });

        public ICommand ConfirmCommand => new RelayCommand(() =>
        {
            
        });

        public ICommand SkipCommand => new RelayCommand(() =>
        {
            
            new MainPage(SelectedApi);
        });
    }
}

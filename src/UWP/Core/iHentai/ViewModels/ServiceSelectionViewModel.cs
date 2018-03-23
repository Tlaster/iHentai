using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Database;
using iHentai.Database.Models;
using iHentai.Mvvm;
using iHentai.Paging;
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

        public ServiceSelectionBannerModel SelectedService { get; set; }

        public string Title { get; } = Package.Current.DisplayName;

        public void Confirm()
        {
            var service = SelectedService.ServiceType;
            Navigate(Singleton<ApiContainer>.Instance.Navigation(service), service).FireAndForget();
            Frame.Navigated += FrameOnNavigated;
        }

        private void FrameOnNavigated(object sender, HentaiNavigationEventArgs hentaiNavigationEventArgs)
        {
            Frame.Navigated -= FrameOnNavigated;
            Frame.ClearBackStack();
        }

        public void GoSettings()
        {
            Navigate<SettingsViewModel>().FireAndForget();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Conet.Apis.Core;
using iHentai.Apis.Core;
using iHentai.Core.ViewModels;
using iHentai.Mvvm;
using iHentai.Pages;
using iHentai.Services;
using iHentai.ViewModels;

namespace iHentai
{
    public sealed partial class App : MvvmApplication, IApiApplication
    {
        private readonly Lazy<ActivationService> _activationService;

        public App()
        {
            InitializeComponent();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        private ActivationService ActivationService => _activationService.Value;

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
                await ActivationService.ActivateAsync(args);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(RootPage));
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        public override IEnumerable<Assembly> MvvmViewAssemblies()
        {
            yield return typeof(GalleryViewModel).GetTypeInfo().Assembly;
        }

        public IEnumerable<Assembly> GetApiAssemblies()
        {
            yield return typeof(IHentaiApi).GetTypeInfo().Assembly;
            yield return typeof(IConetApi).GetTypeInfo().Assembly;
        }
    }
}
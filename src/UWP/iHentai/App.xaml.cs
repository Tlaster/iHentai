﻿using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using iHentai.Services;
using iHentai.ViewModels;
using iHentai.Views;
using RootPage = iHentai.Pages.RootPage;

namespace iHentai
{
    public sealed partial class App : Application
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
    }
}
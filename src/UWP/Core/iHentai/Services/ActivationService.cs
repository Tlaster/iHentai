using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Flurl.Http;
using iHentai.Activation;
using iHentai.Apis.Core;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Paging;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private readonly UIElement _shell;

        public ActivationService(App app, Type defaultNavItem, UIElement shell = null)
        {
            _app = app;
            _shell = shell ?? new HentaiFrame();
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = _shell;
                    //NavigationService.NavigationFailed += (sender, e) => throw e.Exception;
                    //NavigationService.Navigated += Frame_Navigated;
                    //if (SystemNavigationManager.GetForCurrentView() != null)
                    //    SystemNavigationManager.GetForCurrentView().BackRequested += ActivationService_BackRequested;
                }
            }

            var activationHandler = GetActivationHandlers()
                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
                await activationHandler.HandleAsync(activationArgs);

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                    await defaultHandler.HandleAsync(activationArgs);

                // Ensure the current window is active
                Window.Current.Activate();

                ExtendAcrylicIntoTitleBar();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private async Task InitializeAsync()
        {
            Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasks();
            ThemeSelectorService.Initialize();
            await ImageCache.Instance.InitializeAsync(httpMessageHandler: new ApiHttpClient());
            FlurlHttp.Configure(c => c.HttpClientFactory = new ApiHttpClientFactory());
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            ThemeSelectorService.SetRequestedTheme();
            await WhatsNewDisplayService.ShowIfAppropriateAsync();
            await FirstRunDisplayService.ShowIfAppropriateAsync();
            await Task.CompletedTask;
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<ToastNotificationsService>.Instance;
            yield return Singleton<BackgroundTaskService>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        //private void Frame_Navigated(object sender, HentaiNavigationEventArgs e)
        //{
        //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack
        //        ? AppViewBackButtonVisibility.Visible
        //        : AppViewBackButtonVisibility.Collapsed;
        //}

        //private void ActivationService_BackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    //if (NavigationService.CanGoBack)
        //    //{
        //    //    NavigationService.GoBack();
        //    //    e.Handled = true;
        //    //}
        //}
    }
}
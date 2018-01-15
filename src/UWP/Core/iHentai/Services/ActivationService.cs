using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using FFImageLoading.Config;
using FFImageLoading.Helpers;
using FFImageLoading.Views;
using Flurl.Http;
using Humanizer;
using iHentai.Activation;
using iHentai.Basic.Helpers;
using iHentai.Paging;
using iHentai.Views;
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
            _shell = shell;
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
                    Window.Current.Content = _shell ?? new RootView((activationArgs as IActivatedEventArgs)?.SplashScreen, _defaultNavItem);
            }

            var activationHandler = GetActivationHandlers()
                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
                await activationHandler.HandleAsync(activationArgs);

            if (IsInteractive(activationArgs))
            {
                //var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                //if (defaultHandler.CanHandle(activationArgs))
                //    await defaultHandler.HandleAsync(activationArgs);

                // Ensure the current window is active
                Window.Current.Activate();
                
                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync()
        {
            Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasks();
            ThemeSelectorService.Initialize();
            await ImageCache.Instance.InitializeAsync(httpMessageHandler: Singleton<ApiHttpClient>.Instance);
            FFImageLoading.ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = new HttpClient(Singleton<ApiHttpClient>.Instance),
                HttpHeadersTimeout = 10,
                HttpReadTimeout = 30,
                AnimateGifs = true,
                FadeAnimationEnabled = true,
                FadeAnimationForCachedImages = false,
                MaxMemoryCacheSize = Convert.ToInt32(20.Megabytes().Bytes),
                ExecuteCallbacksOnUIThread = true,
                ClearMemoryCacheOnOutOfMemory = true,
                SchedulerMaxParallelTasks = Math.Max(2, (int)(Environment.ProcessorCount * 2d)),
                MainThreadDispatcher = new HentaiThreadDispatcher()
            });
            FlurlHttp.Configure(c => c.HttpClientFactory = Singleton<ApiHttpClientFactory>.Instance);
        }

        private async Task StartupAsync()
        {
            ThemeSelectorService.SetRequestedTheme();
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
    }

    public class HentaiThreadDispatcher : IMainThreadDispatcher
    {
        //private volatile CoreDispatcher _dispatcher;

        public async void Post(Action action)
        {
            if (action == null)
                return;

            var _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;//RunAsync(CoreDispatcherPriority.Normal, () => action());
            if (_dispatcher == null)
            {
                _dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            }

            // already in UI thread:
            if (_dispatcher.HasThreadAccess)
            {
                action();
            }
            // not in UI thread, ensuring UI thread:
            else
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
                //await CoreApplication.GetCurrentView().Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => action());
            }
        }

        public Task PostAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            Post(() =>
            {
                try
                {
                    action?.Invoke();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }

        public Task PostAsync(Func<Task> action)
        {
            var tcs = new TaskCompletionSource<bool>();
            Post(async () =>
            {
                try
                {
                    await action?.Invoke();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }


}
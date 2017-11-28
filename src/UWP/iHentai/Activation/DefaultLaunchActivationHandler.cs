using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using iHentai.Mvvm;

namespace iHentai.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly Type _navElement;

        public DefaultLaunchActivationHandler(Type navElement)
        {
            _navElement = navElement;
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            var info = _navElement.GetTypeInfo();
            if (info.IsAssignableFrom(typeof(ViewModel).GetTypeInfo()) || info.IsSubclassOf(typeof(ViewModel)))
                NavigationService.NavigateViewModel(_navElement);
            else
                NavigationService.Navigate(_navElement, args.Arguments);

            // TODO WTS: This is a sample on how to show a toast notification.
            // You can use this sample to create toast notifications where needed in your app.
            //Singleton<ToastNotificationsService>.Instance.ShowToastNotificationSample();
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null;
        }
    }
}
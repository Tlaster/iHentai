using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Paging;
using iHentai.Views;

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
            if (ReflectionHelper.ImplementsGenericDefinition(_navElement, typeof(IMvvmView<>), out var vmType))
            {
                ((Window.Current.Content as RootView).FindName("RootFrame") as HentaiFrame)?.NavigateAsync(_navElement, Activator.CreateInstance(vmType.GetGenericArguments().FirstOrDefault()));
            }
            else
            {
                ((Window.Current.Content as RootView).FindName("RootFrame") as HentaiFrame)?.NavigateAsync(_navElement);
            }
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return Window.Current.Content is RootView;
        }
    }
}
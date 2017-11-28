using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Mvvm
{
    public static class NavigationService
    {
        private static Frame _frame;

        public static Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    _frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return _frame;
            }

            set
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
            }
        }

        public static bool CanGoBack => Frame.CanGoBack;

        public static bool CanGoForward => Frame.CanGoForward;
        public static event NavigatedEventHandler Navigated;

        public static event NavigationFailedEventHandler NavigationFailed;

        public static void GoBack()
        {
            Frame.GoBack();
        }

        public static void GoForward()
        {
            Frame.GoForward();
        }

        public static bool Navigate(Type pageType, object parameter = null,
            NavigationTransitionInfo infoOverride = null)
        {
            // Don't open the same page multiple times
            return Frame.Content?.GetType() != pageType && Frame.Navigate(pageType, parameter, infoOverride);
        }

        public static bool Navigate<T>(object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : Page
        {
            return Navigate(typeof(T), parameter, infoOverride);
        }

        public static bool NavigateViewModel<T>(params object[] args)
            where T : class
        {
            return NavigateViewModel(typeof(T), args);
        }

        public static bool NavigateViewModel(Type vmType, params object[] args)
        {
            var pInfo = vmType.GetTypeInfo();
            var uwpPage = typeof(Page).GetTypeInfo();
            var attr = pInfo.GetCustomAttribute<PageAttribute>();

            if (pInfo.IsSubclassOf(typeof(ViewModel)) && attr != null)
            {
                var vm = Activator.CreateInstance(vmType, args) as ViewModel;
                return Navigate(attr.PageType, new MvvmBundle {ViewModel = vm});
            }
            if (pInfo.IsAssignableFrom(uwpPage) || pInfo.IsSubclassOf(typeof(Page)))
                return Navigate(vmType);
            throw new ArgumentException("Page Type must be based on Xamarin.Forms.Page");
        }

        private static void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
                _frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        private static void UnregisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
                _frame.NavigationFailed -= Frame_NavigationFailed;
            }
        }

        private static void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private static void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(sender, e);
        }
    }
}
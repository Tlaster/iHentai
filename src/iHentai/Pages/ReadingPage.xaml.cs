using Windows.ApplicationModel.Core;
using Windows.Devices.Input;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using iHentai.ViewModels;
using Visibility = Windows.UI.Xaml.Visibility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReadingPage : Page
    {
        private readonly DisplayRequest _display;
        internal ReadingViewModel ViewModel { get; private set; }

        public ReadingPage()
        {
            this.InitializeComponent();
            _display = new DisplayRequest();
            var titleBarHeight = CoreApplication.GetCurrentView().TitleBar.Height;
            TopBarPointerEnterRegion.Height += titleBarHeight;
            TopReadingControl.Padding = new Thickness(8, 8 + titleBarHeight, 8, 8);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = e.Parameter;
            if (e.Parameter is ReadingViewModel readingViewModel)
            {
                ViewModel = readingViewModel;
            }
            _display.RequestActive();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _display.RequestRelease();
        }
        
        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ReadingControl.Visibility = ReadingControl.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ReadingControl_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                e.Handled = true;
                ReadingControl.Visibility = Visibility.Visible;
            }
        }

        private void ReadingControl_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                e.Handled = true;
                ReadingControl.Visibility = Visibility.Collapsed;
            }
        }
        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.ReloadCurrent();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void OnPreviousClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Mouse)
            {
                e.Handled = true;
                ViewModel.Previous();
            }
        }

        private void OnNextClicked(object sender, TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Mouse)
            {
                e.Handled = true;
                ViewModel.Next();
            }
        }
    }
}

using Windows.ApplicationModel.Core;
using Windows.Devices.Input;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using iHentai.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReadingPage : Page
    {
        private readonly DisplayRequest _display;
        private readonly CoreApplicationViewTitleBar _titleBar;

        public ReadingPage()
        {
            InitializeComponent();
            _display = new DisplayRequest();
            _titleBar = CoreApplication.GetCurrentView().TitleBar;
            UpdateTitleBarHeight();
            _titleBar.IsVisibleChanged += TitleBarOnIsVisibleChanged;
            SharedShadow.Receivers.Add(ShadowBackgroundGrid);
        }

        internal ReadingViewModel ViewModel { get; private set; }

        private void TitleBarOnIsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarHeight();
        }

        private void UpdateTitleBarHeight()
        {
            if (_titleBar.IsVisible)
            {
                var titleBarHeight = _titleBar.Height;
                TitleBarBorder.Height = titleBarHeight;
                TitleBarPlaceHolder.Height = titleBarHeight;
                TopBarPointerEnterRegion.Height = 40 + titleBarHeight;
                BackButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackButton.Visibility = Visibility.Visible;
                const int titleBarHeight = 0;
                TitleBarBorder.Height = titleBarHeight;
                TitleBarPlaceHolder.Height = titleBarHeight;
                TopBarPointerEnterRegion.Height = 40 + titleBarHeight;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = e.Parameter;
            if (e.Parameter is ReadingViewModel readingViewModel)
            {
                ViewModel = readingViewModel;
                ViewModel.Init();
            }

            _display.RequestActive();
            Window.Current.SetTitleBar(TitleBarBorder);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _display.RequestRelease();
            _titleBar.IsVisibleChanged -= TitleBarOnIsVisibleChanged;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ReadingControl.Visibility = ReadingControl.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
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

        private void OnLeftClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FlowDirection == FlowDirection.LeftToRight)
            {
                ViewModel.Previous();
            }
            else
            {
                ViewModel.Next();
            }
        }

        private void OnRightClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FlowDirection == FlowDirection.LeftToRight)
            {
                ViewModel.Next();
            }
            else
            {
                ViewModel.Previous();
            }
        }
    }
}
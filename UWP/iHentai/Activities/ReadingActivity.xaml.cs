using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Common.Tab;
using iHentai.ViewModels;
using Microsoft.Toolkit.Helpers;
using Windows.System.Display;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Activities
{

    class ReadingModeTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return base.SelectTemplateCore(item, container);
        }
    }

    internal partial class ReadingActivity
    {
        private DisplayRequest _display;

        public ReadingActivity()
        {
            InitializeComponent();
        }

        public override ITabViewModel TabViewModel => ViewModel;
        public ReadingViewModel ViewModel { get; private set; }

        protected internal override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            if (parameter is ReadingViewModel viewModel)
            {
                ViewModel = viewModel;
            }
            SizeChanged += ReadingActivity_SizeChanged;
            _display = new DisplayRequest();
        }

        protected internal override void OnResume()
        {
            base.OnResume();
            _display.RequestActive();
        }

        protected internal override void OnPause()
        {
            base.OnPause();
            _display.RequestRelease();
        }

        private void ReadingActivity_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ViewModel.Images == null)
            {
                return;
            }
            if (e.NewSize.Height / e.NewSize.Width >= 1)
            {
                ViewModel.ViewMode = ReadingViewMode.Flip;
            } 
            else
            {
                ViewModel.ViewMode = ReadingViewMode.Book;
            }
        }

        protected internal override void OnDestroy()
        {
            base.OnDestroy();
            SizeChanged -= ReadingActivity_SizeChanged;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ReadingControl.Visibility = ReadingControl.IsVisible() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ReadingControl_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                e.Handled = true;
                if (!ReadingControl.IsVisible())
                {
                    ReadingControl.Visibility = Visibility.Visible;
                }
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

        private void RadioMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (Enum.TryParse<ReadingViewMode>(element.Tag.ToString(), out var mode))
                {
                    ViewModel.ViewMode = mode;
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                switch (element.Tag)
                {
                    case "LTR":
                        ViewModel.FlowDirection = FlowDirection.LeftToRight;
                        break;
                    case "RTL":
                        ViewModel.FlowDirection = FlowDirection.RightToLeft;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
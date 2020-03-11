using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace iHentai.Controls
{
  
    [ContentProperty(Name = nameof(Content))]
    public sealed class CardView : Control
    {
        public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register(
            nameof(IsAnimationEnabled), typeof(bool), typeof(CardView), new PropertyMetadata(true));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(UIElement), typeof(CardView), new PropertyMetadata(default));

        public static readonly DependencyProperty IsShadowOnlyProperty = DependencyProperty.Register(
            nameof(IsShadowOnly), typeof(bool), typeof(CardView), new PropertyMetadata(default));

        public CardView()
        {
            DefaultStyleKey = typeof(CardView);
        }

        public bool IsShadowOnly
        {
            get => (bool) GetValue(IsShadowOnlyProperty);
            set => SetValue(IsShadowOnlyProperty, value);
        }

        public bool IsAnimationEnabled
        {
            get => (bool) GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        public UIElement Content
        {
            get => (UIElement) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private DropShadowPanel ShadowPanel { get; set; }

        private Grid RootGrid { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            RootGrid = GetTemplateChild("RootGrid") as Grid;
            ShadowPanel = GetTemplateChild("ShadowPanel") as DropShadowPanel;
            if (IsShadowOnly && ShadowPanel != null)
            {
                ShadowPanel.Visibility = Visibility.Visible;
                ShadowPanel.Opacity = 1F;
            }
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            if (!IsAnimationEnabled || IsShadowOnly) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                ShadowPanel.Fade(0F, 150D, easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (!IsAnimationEnabled || IsShadowOnly) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == PointerDeviceType.Pen)
            {
                ShadowPanel.Visibility = Visibility.Visible;
                ShadowPanel.Fade(1F, 300D, easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
                RootGrid.Scale(1.1F, 1.1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
            }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!IsAnimationEnabled || IsShadowOnly) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                RootGrid.Scale(0.9F, 0.9F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
            else
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 300D,
                    easingMode: EasingMode.EaseOut,
                    easingType: EasingType.Cubic).Start();
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (!IsAnimationEnabled || IsShadowOnly) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                RootGrid.Scale(1F, 1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
            else
                RootGrid.Scale(1.1F, 1.1F, Convert.ToSingle(RootGrid.ActualWidth / 2F),
                    Convert.ToSingle(RootGrid.ActualHeight / 2F), 150D,
                    easingMode: EasingMode.EaseIn,
                    easingType: EasingType.Cubic).Start();
        }
    }
}

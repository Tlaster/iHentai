using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(iHentai.UWP.Renderers.ActivityIndicatorRenderer))]
namespace iHentai.UWP.Renderers
{
    public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, ProgressRing>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
        {
            base.OnElementChanged(e);
            if (Control == null && Element != null)
            {
                SetNativeControl(new ProgressRing());
                Control.IsActive = Element.IsRunning;
                UpdateColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName)
                Control.IsActive = Element.IsRunning;
            else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
                UpdateColor();
        }

        void UpdateColor()
        {
            Color color = Element.Color;
            if (color == Color.Default)
                Control.ClearValue(Windows.UI.Xaml.Controls.Control.ForegroundProperty);
            else
                Control.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(
                    Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255),
                        (byte)(color.B * 255)));
        }

    }
    //public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, ProgressRing>
    //{
    //    protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
    //    {
    //        base.OnElementChanged(e);

    //        if (e.NewElement != null)
    //        {
    //            if (Control == null)
    //            {
    //                SetNativeControl(new ProgressRing { IsActive = true });
    //                Control.Loaded += OnControlLoaded;
    //            }

    //            // UpdateColor() called when loaded to ensure we can cache dynamic default colors
    //            UpdateIsRunning();
    //        }
    //    }

    //    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnElementPropertyChanged(sender, e);

    //        if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName || e.PropertyName == VisualElement.OpacityProperty.PropertyName)
    //            UpdateIsRunning();
    //        else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
    //            UpdateColor();
    //    }

    //    private void OnControlLoaded(object sender, RoutedEventArgs routedEventArgs)
    //    {
    //        UpdateColor();
    //    }

    //    private void UpdateColor()
    //    {
    //        var color = Element.Color;

    //        if (color.IsDefault)
    //        {
    //            Control.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(
    //                Windows.UI.Color.FromArgb((byte)(Color.Accent.A * 255), (byte)(Color.Accent.R * 255), (byte)(Color.Accent.G * 255),
    //                    (byte)(Color.Accent.B * 255)));
    //        }
    //        else
    //        {
    //            Control.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(
    //                Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255),
    //                    (byte)(color.B * 255)));
    //        }
    //    }

    //    private void UpdateIsRunning()
    //    {
    //        Control.Opacity = Element.IsRunning ? Element.Opacity : 0;
    //    }
    //}
}

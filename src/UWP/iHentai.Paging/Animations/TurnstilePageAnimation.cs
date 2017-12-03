using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace iHentai.Paging.Animations
{
    public class TurnstilePageAnimation : IPageAnimation
    {
        public TurnstilePageAnimation()
        {
            Duration = TimeSpan.FromMilliseconds(150);
        }


        public TimeSpan Duration { get; set; }


        public bool UseBitmapCacheMode { get; set; }


        public PageInsertionMode PageInsertionMode => PageInsertionMode.Sequential;


        public Task AnimateForwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            return AnimateAsync(previousPage, 5, 75, 1, 0);
        }


        public Task AnimateForwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            return AnimateAsync(nextPage, -75, 0, 0, 1);
        }


        public Task AnimateBackwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            return AnimateAsync(previousPage, -5, -75, 1, 0);
        }


        public Task AnimateBackwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            return AnimateAsync(nextPage, 75, 0, 0, 1);
        }

        private Task AnimateAsync(FrameworkElement page, double fromRotation, double toRotation, double fromOpacity,
            double toOpacity)
        {
            if (page == null)
                return Task.FromResult<object>(null);

            CacheMode originalCacheMode = null;
            if (UseBitmapCacheMode && !(page.CacheMode is BitmapCache))
            {
                originalCacheMode = page.CacheMode;
                page.CacheMode = new BitmapCache();
            }

            page.Opacity = 1;
            page.Projection = new PlaneProjection {CenterOfRotationX = 0};

            var story = new Storyboard();

            var turnstileAnimation = new DoubleAnimation
            {
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut},
                Duration = Duration,
                From = fromRotation,
                To = toRotation
            };
            Storyboard.SetTargetProperty(turnstileAnimation, "(UIElement.Projection).(PlaneProjection.RotationY)");
            Storyboard.SetTarget(turnstileAnimation, page);

            var opacityAnimation = new DoubleAnimation
            {
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut},
                Duration = Duration,
                From = fromOpacity,
                To = toOpacity
            };
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
            Storyboard.SetTarget(opacityAnimation, page);

            story.Children.Add(turnstileAnimation);
            story.Children.Add(opacityAnimation);

            var completion = new TaskCompletionSource<object>();
            story.Completed += delegate
            {
                ((PlaneProjection) page.Projection).RotationY = toRotation;

                page.Opacity = 1.0;
                page.CacheMode = originalCacheMode;

                completion.SetResult(null);
            };
            story.Begin();
            return completion.Task;
        }
    }
}
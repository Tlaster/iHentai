using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace iHentai.Paging.Animations
{
    public class ScalePageTransition : IPageAnimation
    {
        public ScalePageTransition()
        {
            Duration = TimeSpan.FromMilliseconds(80);
        }


        public TimeSpan Duration { get; set; }


        public PageInsertionMode PageInsertionMode => PageInsertionMode.ConcurrentAbove;


        public Task AnimateForwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 1;
            nextPage.Opacity = 0;
            return AnimateAsync(previousPage, 1, 1.1, 1, 0);
        }


        public Task AnimateForwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 0;
            nextPage.Opacity = 1;
            return AnimateAsync(nextPage, 0.9, 1, 0, 1);
        }


        public Task AnimateBackwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 1;
            nextPage.Opacity = 0;
            return AnimateAsync(previousPage, 1, 0.9, 1, 0);
        }


        public Task AnimateBackwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 0;
            nextPage.Opacity = 1;
            return AnimateAsync(nextPage, 1.1, 1, 0, 1);
        }

        private Task AnimateAsync(FrameworkElement page, double fromPreviousPage, double toPreviousPage,
            double opacityFrom, double opacityTo)
        {
            if (page == null)
                return Task.FromResult<object>(null);

            page.RenderTransform = new ScaleTransform
            {
                CenterX = page.ActualWidth / 2,
                CenterY = page.ActualHeight / 2
            };

            var story = new Storyboard();

            var scaleXAnimation = new DoubleAnimation
            {
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut},
                Duration = Duration,
                From = fromPreviousPage,
                To = toPreviousPage
            };
            Storyboard.SetTargetProperty(scaleXAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(scaleXAnimation, page);

            var scaleYAnimation = new DoubleAnimation
            {
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut},
                Duration = Duration,
                From = fromPreviousPage,
                To = toPreviousPage
            };
            Storyboard.SetTargetProperty(scaleYAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
            Storyboard.SetTarget(scaleYAnimation, page);

            var opacityAnimation = new DoubleAnimation
            {
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut},
                Duration = Duration,
                From = opacityFrom,
                To = opacityTo
            };
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
            Storyboard.SetTarget(opacityAnimation, page);

            story.Children.Add(scaleXAnimation);
            story.Children.Add(scaleYAnimation);
            story.Children.Add(opacityAnimation);

            var completion = new TaskCompletionSource<object>();
            story.Completed += delegate { completion.SetResult(null); };
            story.Begin();
            return completion.Task;
        }
    }
}
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace iHentai.Paging.Animations
{
    public class FadeAnimation : IPageAnimation
    {
        public double Duration { get; set; } = 250d;
        public PageInsertionMode PageInsertionMode => PageInsertionMode.ConcurrentAbove;

        public async Task AnimateForwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 1d;
            nextPage.Opacity = 0d;
            await previousPage.Fade(0, Duration / 2).StartAsync();
        }

        public async Task AnimateForwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 0;
            nextPage.Opacity = 1;
            await nextPage.Fade(1, Duration / 2).StartAsync();
        }

        public async Task AnimateBackwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 1;
            nextPage.Opacity = 0;
            await previousPage.Fade(0, Duration / 2).StartAsync();
        }

        public async Task AnimateBackwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage)
        {
            previousPage.Opacity = 0;
            nextPage.Opacity = 1;
            await nextPage.Fade(1, Duration / 2).StartAsync();
        }
    }
}
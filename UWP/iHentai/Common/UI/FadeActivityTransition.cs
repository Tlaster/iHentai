using System.Threading.Tasks;
using Windows.UI.Xaml;
using iHentai.Controls.Paging;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace iHentai.Common.UI
{
    internal class FadeActivityTransition : IActivityTransition
    {
        private readonly double _duration = 250d;
        public ActivityInsertionMode InsertionMode { get; } = ActivityInsertionMode.NewBelow;

        public async Task OnStart(FrameworkElement newActivity, FrameworkElement currentActivity)
        {
            if (currentActivity != null)
            {
                currentActivity.Opacity = 1f;
                currentActivity.Fade(duration: _duration).Start();
            }

            if (newActivity != null)
            {
                newActivity.Opacity = 0f;
                await newActivity.Fade(1, duration: _duration).StartAsync();
            }
        }

        public async Task OnClose(FrameworkElement closeActivity, FrameworkElement previousActivity)
        {
            if (closeActivity != null)
            {
                closeActivity.Opacity = 1f;
                closeActivity.Fade(duration: _duration).Start();
            }

            if (previousActivity != null)
            {
                previousActivity.Opacity = 0f;
                await previousActivity.Fade(1, duration: _duration).StartAsync();
            }
        }
    }
}

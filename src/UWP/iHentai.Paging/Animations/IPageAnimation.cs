using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHentai.Paging.Animations
{
    public interface IPageAnimation
    {
        PageInsertionMode PageInsertionMode { get; }

        Task AnimateForwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage);

        Task AnimateForwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage);

        Task AnimateBackwardNavigatingFromAsync(FrameworkElement previousPage, FrameworkElement nextPage);

        Task AnimateBackwardNavigatedToAsync(FrameworkElement previousPage, FrameworkElement nextPage);
    }
}
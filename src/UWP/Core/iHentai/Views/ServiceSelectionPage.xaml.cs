using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using iHentai.Mvvm;
using iHentai.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServiceSelectionPage : IMvvmView<ServiceSelectionViewModel>
    {
        public ServiceSelectionPage()
        {
            InitializeComponent();
        }

        public new ServiceSelectionViewModel ViewModel
        {
            get => (ServiceSelectionViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("service_text",
                (ServicesCarousel.ContainerFromIndex(ServicesCarousel.SelectedIndex) as UIElement)
                .FindDescendant<TextBlock>());
            ViewModel.Confirm();
        }
    }
}
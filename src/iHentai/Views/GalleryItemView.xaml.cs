using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using iHentai.Services.Models.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace iHentai.Views
{
    public sealed partial class GalleryItemView : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(IGallery), typeof(GalleryItemView), new PropertyMetadata(default(IGallery)));

        public IGallery Data
        {
            get { return (IGallery) GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public GalleryItemView()
        {
            this.InitializeComponent();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}

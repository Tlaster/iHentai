using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.EHentai
{
    internal class GalleryDetailViewModel : TabViewModelBase
    {
        public GalleryDetailViewModel(EHGallery gallery)
        {
            Gallery = gallery;
            Title = gallery.Title;
            Init();
        }

        public EHGalleryDetail Detail { get; private set; }
        public EHGallery Gallery { get; }

        private async void Init()
        {
            IsLoading = true;
            Detail = await Singleton<EHApi>.Instance.Detail(Gallery.Link);
            IsLoading = false;
        }
    }
}
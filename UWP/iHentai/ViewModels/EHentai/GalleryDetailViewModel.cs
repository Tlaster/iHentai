using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Helpers;
using PropertyChanged;

namespace iHentai.ViewModels.EHentai
{
    internal class GalleryDetailViewModel : TabViewModelBase
    {
        private readonly EHApi _api;

        public GalleryDetailViewModel(EHGallery gallery, EHApi api)
        {
            _api = api;
            Gallery = gallery;
            Title = gallery.Title;
            Init();
        }

        public GalleryDetailViewModel(string gallery, EHApi api)
        {
            _api = api;
            Init(gallery);
        }

        [DependsOn(nameof(Detail))] public int ThumbWidth => Gallery?.ThumbWidth ?? Detail?.ThumbWidth ?? 0;

        [DependsOn(nameof(Detail))] public int ThumbHeight => Gallery?.ThumbHeight ?? Detail?.ThumbHeight ?? 0;

        [DependsOn(nameof(Detail))] public string Thumb => Gallery?.Thumb ?? Detail?.Thumb;

        [DependsOn(nameof(Detail))] public string GalleryTitle => Gallery?.Title ?? Detail?.Name;

        [DependsOn(nameof(Detail))] public string Uploader => Gallery?.Uploader ?? Detail?.Uploader;

        [DependsOn(nameof(Detail))]
        public EHCategory Category => Gallery?.Category ?? Detail?.Category ?? EHCategory.Doujinshi;

        public EHGalleryDetail Detail { get; private set; }
        public EHGallery Gallery { get; }

        private async void Init(string gallery)
        {
            IsLoading = true;
            Detail = await _api.Detail(gallery);
            IsLoading = false;
        }

        private async void Init()
        {
            IsLoading = true;
            Detail = await _api.Detail(Gallery.Link);
            IsLoading = false;
        }
    }
}
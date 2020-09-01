using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using PropertyChanged;

namespace iHentai.ViewModels.EHentai
{
    internal class GalleryDetailViewModel : TabViewModelBase
    {
        public GalleryDetailViewModel(EHGallery gallery, EHApi api)
        {
            Api = api;
            Gallery = gallery;
            Title = gallery.Title;
            Init();
        }

        public GalleryDetailViewModel(string gallery, EHApi api)
        {
            Api = api;
            Init(gallery);
        }

        public EHApi Api { get; }

        [DependsOn(nameof(Detail))] public int ThumbWidth => Gallery?.ThumbWidth ?? Detail?.ThumbWidth ?? 0;

        [DependsOn(nameof(Detail))] public int ThumbHeight => Gallery?.ThumbHeight ?? Detail?.ThumbHeight ?? 0;

        [DependsOn(nameof(Detail))] public string Thumb => Gallery?.Thumb ?? Detail?.Thumb;

        [DependsOn(nameof(Detail))] public string GalleryTitle => Gallery?.Title ?? Detail?.Name;

        [DependsOn(nameof(Detail))] public string Uploader => Gallery?.Uploader ?? Detail?.Uploader;

        [DependsOn(nameof(Detail))]
        public EHCategory Category => Gallery?.Category ?? Detail?.Category ?? EHCategory.Doujinshi;

        public EHGalleryDetail Detail { get; private set; }
        public EHGallery Gallery { get; }
        public string Link { get; private set; }

        private async void Init(string gallery)
        {
            IsLoading = true;
            Link = gallery;
            Detail = await Api.Detail(gallery);
            IsLoading = false;
        }

        private async void Init()
        {
            IsLoading = true;
            Link = Gallery.Link;
            Detail = await Api.Detail(Gallery.Link);
            IsLoading = false;
        }
    }
}
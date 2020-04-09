using iHentai.Services.Core;
using PropertyChanged;

namespace iHentai.ViewModels.Generic
{
    internal class MangaDetailViewModel : TabViewModelBase
    {
        public MangaDetailViewModel(IMangaApi api, IMangaGallery gallery)
        {
            Api = api;
            Gallery = gallery;
            Init();
        }

        public IMangaApi Api { get; }
        public IMangaGallery Gallery { get; }
        public IMangaDetail Detail { get; private set; }

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string GalleryTitle => Gallery?.Title ?? Detail?.Title;

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string Thumb => Detail?.Thumb ?? Gallery?.Thumb;

        private async void Init()
        {
            IsLoading = true;
            Title = GalleryTitle;
            Detail = await Api.Detail(Gallery);
            IsLoading = false;
        }
    }
}
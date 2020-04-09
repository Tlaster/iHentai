using System.Linq;
using iHentai.Services.Core;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Helpers;
using PropertyChanged;

namespace iHentai.ViewModels.Manhuagui
{
    internal class ManhuaguiDetailViewModel : TabViewModelBase
    {
        private readonly IMangaApi _api;
        public ManhuaguiDetailViewModel(IMangaApi api, IMangaGallery gallery)
        {
            _api = api;
            Gallery = gallery;
            Init();
        }


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
            Detail = await _api.Detail(Gallery);
            IsLoading = false;
        }
    }
}
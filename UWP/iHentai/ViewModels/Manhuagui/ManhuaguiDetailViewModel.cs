using System.Linq;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Helpers;
using PropertyChanged;

namespace iHentai.ViewModels.Manhuagui
{
    internal class ManhuaguiDetailViewModel : TabViewModelBase
    {
        public ManhuaguiDetailViewModel(ManhuaguiGallery gallery)
        {
            Gallery = gallery;
            Init();
        }

        public ManhuaguiDetailViewModel(string link)
        {
            Init(link);
        }

        public ManhuaguiGallery Gallery { get; }
        public string Link { get; private set; }
        public ManhuaguiGalleryDetail Detail { get; private set; }

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string GalleryTitle => Gallery?.Title ?? Detail?.Title;

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string Thumb => Detail?.Image ?? Gallery?.Thumb;


        private async void Init(string gallery)
        {
            IsLoading = true;
            Link = gallery;
            Detail = await Singleton<ManhuaguiApi>.Instance.Detail(gallery);
            Title = GalleryTitle;
            IsLoading = false;
        }

        private async void Init()
        {
            IsLoading = true;
            Link = Gallery.Link;
            Title = GalleryTitle;
            Detail = await Singleton<ManhuaguiApi>.Instance.Detail(Gallery.Link);
            IsLoading = false;
        }
    }
}
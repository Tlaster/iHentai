using System.Linq;
using iHentai.Data.Models;
using iHentai.ReadingImages;
using iHentai.Services;

namespace iHentai.ViewModels.Local
{
    class LocalReadingViewModel : ReadingViewModel
    {
        public LocalGalleryModel Gallery { get; }
        public LocalReadingViewModel(LocalGalleryModel gallery)
        {
            Gallery = gallery;
            Init();
        }

        private async void Init()
        {
            IsLoading = true;
            Title = Gallery.Title ?? "";
            var files = await LocalLibraryManager.Instance.GetGalleryImages(Gallery);
            Images = files.Select(it => new LocalReadingImage(it)).ToList();
            IsLoading = false;
        }
    }
}

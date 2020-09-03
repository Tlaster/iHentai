using System.Linq;
using iHentai.Data.Models;
using iHentai.ReadingImages;
using iHentai.Services;

namespace iHentai.ViewModels.Local
{
    class LocalReadingViewModel : ReadingViewModel
    {
        public LocalGalleryModel Gallery { get; }
        public LocalReadingViewModel(LocalGalleryModel gallery) : base(gallery.Title)
        {
            Gallery = gallery;
            Init();
        }

        private async void Init()
        {
            IsLoading = true;
            var files = await LocalLibraryManager.Instance.GetGalleryImages(Gallery);
            Images = files.Select(it => new LocalReadingImage(it)).ToList();
            IsLoading = false;
        }
    }
}

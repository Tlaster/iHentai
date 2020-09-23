using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Data.Models;
using iHentai.ReadingImages;
using iHentai.Services;

namespace iHentai.ViewModels.Local
{
    internal class LocalReadingViewModel : ReadingViewModel
    {
        public LocalReadingViewModel(LocalGalleryModel gallery) : base(gallery.Title)
        {
            Gallery = gallery;
        }

        public LocalGalleryModel Gallery { get; }


        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            var files = await LocalLibraryManager.Instance.GetGalleryImages(Gallery);
            return files.Select((it, index) => new LocalReadingImage(it, index)).ToList();
        }
    }
}
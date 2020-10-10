using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Data;
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


        public override void SaveReadingHistory()
        {
            ReadingHistoryDb.Instance.AddOrUpdate(
                Gallery.Title,
                Gallery.Thumb,
                Gallery.Path,
                GalleryType.Script,
                new LocalGalleryHistoryExtra
                {
                    Path = Gallery.Path,
                    Progress = SelectedIndex,
                }.ToJson(),
                "LocalGalleryHistoryExtra"
            );
        }

        protected override int RestoreReadingProgress()
        {
            var item = ReadingHistoryDb.Instance.Source.FirstOrDefault(it =>
                it.GalleryId == Gallery.Path && it.GalleryType == GalleryType.Script &&
                it.ExtraInstance is LocalGalleryHistoryExtra extra && extra.Path == Gallery.Path);
            if (item == null)
            {
                return 0;
            }
            else
            {
                return (item.ExtraInstance as LocalGalleryHistoryExtra)?.Progress ?? 0;
            }
        }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            var files = await LocalLibraryManager.Instance.GetGalleryImages(Gallery);
            return files.Select((it, index) => new LocalReadingImage(it, index)).ToList();
        }
    }
}
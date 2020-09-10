using System.Linq;
using iHentai.ReadingImages;
using iHentai.Services;
using iHentai.Services.Models.Script;

namespace iHentai.ViewModels.Script
{
    internal class ScriptGalleryReadingViewModel : ReadingViewModel
    {
        public ScriptGalleryReadingViewModel(ScriptApi api, ScriptGalleryDetailModel detail) : base(
            detail.Title)
        {
            Api = api;
            Detail = detail;
            Init();
        }

        public ScriptApi Api { get; set; }

        public ScriptGalleryDetailModel Detail { get; }

        private async void Init()
        {
            IsLoading = true;
            var files = await Api.GalleryImagePages(Detail);
            Images = files.Select((it, index) => new ReadingImage(it, index + 1)).ToList();
            IsLoading = false;
        }
    }
}
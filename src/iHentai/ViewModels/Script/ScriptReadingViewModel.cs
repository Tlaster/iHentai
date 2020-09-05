using System.Linq;
using iHentai.ReadingImages;
using iHentai.Services;
using iHentai.Services.Models.Script;

namespace iHentai.ViewModels.Script
{
    internal class ScriptReadingViewModel : ReadingViewModel
    {
        public ScriptReadingViewModel(ScriptApi api, ScriptGalleryDetailModel detail,
            ScriptGalleryChapter chapter) : base(
            detail.Title)
        {
            Api = api;
            Detail = detail;
            Chapter = chapter;
            Init();
        }

        public ScriptApi Api { get; set; }

        public ScriptGalleryChapter Chapter { get; }

        public ScriptGalleryDetailModel Detail { get; }

        private async void Init()
        {
            IsLoading = true;
            var files = await Api.ChapterImages(Chapter);
            Images = files.Select((it, index) => new ReadingImage(it, index + 1)).ToList();
            IsLoading = false;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iHentai.ReadingImages;
using iHentai.Services;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Local;

namespace iHentai.ViewModels.Script
{
    internal class ScriptChapterReadingViewModel : ReadingViewModel
    {
        public ScriptChapterReadingViewModel(ScriptApi api, ScriptGalleryDetailModel detail,
            ScriptGalleryChapter chapter) : base(
            detail.Title)
        {
            Api = api;
            Detail = detail;
            Chapter = chapter;
        }

        public ScriptApi Api { get; set; }

        public ScriptGalleryChapter Chapter { get; }

        public ScriptGalleryDetailModel Detail { get; }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            var files = await Api.ChapterImages(Chapter);
            return files.Select((it, index) => new ReadingImage(it, index + 1)).ToList();
        }
    }
}
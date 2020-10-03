using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Common.Helpers;
using iHentai.ReadingImages;
using iHentai.Services;
using iHentai.Services.Models.Core;
using iHentai.Services.Models.Script;
using iHentai.ViewModels.Local;
using Newtonsoft.Json;
using PropertyChanged;

namespace iHentai.ViewModels.Script
{
    internal class ScriptChapterReadingViewModel : ReadingViewModel, IChapterReadingViewModel
    {
        private ScriptGalleryChapter _chapter;

        public ScriptChapterReadingViewModel(ScriptApi api, ScriptGalleryDetailModel detail,
            ScriptGalleryChapter chapter) : base(
            detail.Title + " - " + chapter.Title)
        {
            Api = api;
            Detail = detail;
            _chapter = chapter;
        }

        public ScriptApi Api { get; set; }

        public ScriptGalleryChapter Chapter
        {
            get => _chapter;
            set
            {
                _chapter = value;
                Title = Detail.Title + " - " + Chapter.Title;
                Init();
            }
        }

        public ScriptGalleryDetailModel Detail { get; }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            List<string> files;
            if (Chapter.Id != null && Detail.Id != null)
            {
                files = await ScriptImageListCache.Instance.GetFromCacheAsync($"{Api.Id}_{Detail.Id}_{Chapter.Id}", Task.Run<Stream>(async () =>
                {
                    var result = await Api.ChapterImages(Chapter);
                    return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
                }));
            }
            else
            {
                files = await Api.ChapterImages(Chapter);
            }
            return files.Select((it, index) => new ReadingImage(it, index + 1)).ToList();
        }


        // Manga chapter is ordering in reverse way, so latest chapter is the first item of the list

        public IMangaChapter CurrentChapter
        {
            get => Chapter;
            set
            {
                if (value is ScriptGalleryChapter newValue)
                {
                    Chapter = newValue;
                }
            }
        }

        public IEnumerable<IMangaChapter> Chapters => Detail.Chapters ?? new List<ScriptGalleryChapter>();

        [DependsOn(nameof(Chapter))]
        public bool HasNext => Detail.Chapters?.FirstOrDefault() != Chapter;
        [DependsOn(nameof(Chapter))]
        public bool HasPrevious => Detail.Chapters?.LastOrDefault() != Chapter;

        public void GoNext()
        {
            if (Detail.Chapters != null)
            {
                var index = Detail.Chapters.IndexOf(Chapter);
                var next = Detail.Chapters.ElementAtOrDefault(index - 1);
                if (next != null)
                {
                    Chapter = next;
                }
            }
        }

        public void GoPrevious()
        {
            if (Detail.Chapters != null)
            {
                var index = Detail.Chapters.IndexOf(Chapter);
                var next = Detail.Chapters.ElementAtOrDefault(index + 1);
                if (next != null)
                {
                    Chapter = next;
                }
            }
        }
    }
}
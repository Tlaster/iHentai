using System.Linq;
using iHentai.Services.Core;

namespace iHentai.ViewModels.Generic
{
    class MangaReadingViewModel : ReadingViewModel
    {
        private readonly IMangaChapter _chapter;
        private readonly IMangaDetail _detail;
        private readonly IMangaApi _api;
        public MangaReadingViewModel(IMangaChapter chapter, IMangaDetail detail, IMangaApi api)
        {
            _chapter = chapter;
            _detail = detail;
            _api = api;
            this.Title = detail.Title;
            Init();
        }

        private async void Init()
        {
            IsLoading = true;
            var images = await _api.ChapterImages(_chapter);
            Images = images.Select((it, index) => new ReadingImage(it, index, _detail.Title)).OfType<IReadingImage>().ToList();
            IsLoading = false;
        }
    }

}

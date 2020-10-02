
using System.Collections.Generic;
using iHentai.Services.Models.Core;

namespace iHentai.ViewModels
{
    interface IChapterReadingViewModel
    {
        IMangaChapter CurrentChapter { get; set; }
        IEnumerable<IMangaChapter> Chapters { get; }
        bool HasNext { get; }
        bool HasPrevious { get; }
        void GoNext();
        void GoPrevious();
    }
}
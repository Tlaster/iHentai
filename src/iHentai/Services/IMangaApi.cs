using System.Collections.Generic;
using System.Threading.Tasks;
using iHentai.Services.Models.Core;

namespace iHentai.Services
{
    public interface IMangaApi
    {
        /// <summary>
        /// </summary>
        /// <param name="page">From 0</param>
        /// <returns></returns>
        Task<IEnumerable<IGallery>> Home(int page);

        //ReadingViewModel? GenerateReadingViewModel(IGallery gallery);
    }

    public interface ISearchableApi
    {
        /// <summary>
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page">From 0</param>
        /// <returns></returns>
        Task<IEnumerable<IGallery>> Search(string keyword, int page);
    }

    public interface IDetailedApi
    {
        Task<IGalleryDetail> Detail(IGallery gallery);

        Task<bool> CheckCanOpenChapter(IMangaChapter chapter);

        //ReadingViewModel? GenerateReadingViewModel(IGalleryDetail detail, IMangaChapter gallery);
        Task<List<string>> ChapterImages(IMangaChapter chapter);
        Task<List<string>> GalleryImagePages(IGalleryDetail detail);
    }
}
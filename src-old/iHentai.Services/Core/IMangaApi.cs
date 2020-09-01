using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHentai.Services.Core
{
    public interface IMangaApi: IApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">From 0</param>
        /// <returns></returns>
        Task<IEnumerable<IMangaGallery>> Home(int page);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page">From 0</param>
        /// <returns></returns>
        Task<IEnumerable<IMangaGallery>> Search(string keyword, int page);

        Task<IMangaDetail> Detail(IMangaGallery gallery);

        Task<List<string>> ChapterImages(IMangaChapter chapter);

        bool CheckCanOpenChapter(IMangaChapter chapter);
        string GetGalleryLink(IMangaGallery gallery);
    }

    public interface IApi
    {
        string Name { get; }
        bool CanHandle(Uri uri);
    }

    public interface IHentaiApi : IApi
    {
        
    }
}
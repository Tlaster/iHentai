using System.Collections.Generic;
using System.Threading.Tasks;

namespace iHentai.Services.Core
{
    interface IMangaApi
    {
        string Name { get; }

        Task<IEnumerable<IMangaGallery>> Home(int page);

        Task<IEnumerable<IMangaGallery>> Search(string keyword, int page);

        Task<IMangaDetail> Detail(IMangaGallery gallery);

        Task<List<string>> ChapterImages(IMangaChapter chapter);
    }
}
using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.NHentai
{
    public class Apis : IHentaiApis
    {
        public string Cookie { get; }
        public string Host { get; } = "https://nhentai.net/";
        public Task<IGalleryModel> Gallery(int page = 0, ISearchOption searchOption = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IGalleryModel> TaggedGallery(string name, int page = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
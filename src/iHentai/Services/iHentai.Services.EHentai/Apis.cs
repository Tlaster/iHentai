using System;
using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.EHentai
{
    public class Apis : IHentaiApis
    {
        public bool IsExhentaiMode { get; set; }
        public string Cookie { get; }
        public string Host => IsExhentaiMode ? "http://g.e-hentai.org/" : "https://exhentai.org/";
        public async Task<IGalleryModel> Gallery(int page = 0, ISearchOption searchOption = null)
        {
            throw new NotImplementedException();
        }

        public Task<IGalleryModel> TaggedGallery(string name, int page = 0)
        {
            throw new NotImplementedException();
        }
    }
}
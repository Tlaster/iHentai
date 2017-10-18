using System;
using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.EHentai
{
    public class Apis : IHentaiApis
    {
        public bool IsExhentaiMode { get; set; }
        public bool FouceLogin { get; } = true;
        public string Cookie { get; }
        public string Host => IsExhentaiMode ? "http://g.e-hentai.org/" : "https://exhentai.org/";
        public IApiConfig ApiConfig { get; } = new UConfig();

        public async Task<IGalleryModel> Gallery(int page = 0, SearchOptionBase searchOption = null)
        {
            throw new NotImplementedException();
        }

        public Task Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IGalleryModel> TaggedGallery(string name, int page = 0)
        {
            throw new NotImplementedException();
        }
    }
}
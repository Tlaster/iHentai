using System.Threading.Tasks;
using iHentai.Services.Core;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Shared.Extensions;
using System.Collections.Generic;

namespace iHentai.Services.NHentai
{
    public class Apis : IHentaiApis
    {
        public bool FouceLogin { get; } = false;
        public string Host { get; } = "https://nhentai.net/";
        public IApiConfig ApiConfig { get; }
        public ISettings Settings { get; }

        public Dictionary<string, string> Cookie => throw new System.NotImplementedException();

        public Task<IEnumerable<IGalleryModel>> Gallery(int page = 0, SearchOptionBase searchOption = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<(bool State, string Message)> Login(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<IGalleryModel> TaggedGallery(string name, int page = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
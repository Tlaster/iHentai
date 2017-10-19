using System.Threading.Tasks;
using iHentai.Services.Core.Models.Interfaces;
using iHentai.Shared.Extensions;
using System.Collections.Generic;

namespace iHentai.Services.Core
{
    public interface IHentaiApis
    {
        bool FouceLogin { get; }
        Dictionary<string, string> Cookie { get; }
        string Host { get; }
        IApiConfig ApiConfig { get; }
        ISettings Settings { get; }
        Task<IEnumerable<IGalleryModel>> Gallery(int page = 0, SearchOptionBase searchOption = null);
        Task<IGalleryModel> TaggedGallery(string name, int page = 0);
        Task<(bool State, string Message)> Login(string userName, string password);
    }
}
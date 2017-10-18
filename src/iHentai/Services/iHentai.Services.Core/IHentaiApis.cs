using System.Threading.Tasks;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.Core
{
    public interface IHentaiApis
    {
        bool FouceLogin { get; }
        string Cookie { get; }
        string Host { get; }
        IApiConfig ApiConfig { get; }
        Task<IGalleryModel> Gallery(int page = 0, SearchOptionBase searchOption = null);
        Task<IGalleryModel> TaggedGallery(string name, int page = 0);
        Task Login(string userName, string password);
    }
}
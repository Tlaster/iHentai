using System.Threading.Tasks;
using iHentai.Services.Core.Models.Interfaces;

namespace iHentai.Services.Core
{
    public interface IHentaiApis
    {
        string Cookie { get; }
        string Host { get; }
        Task<IGalleryModel> Gallery(int page = 0, ISearchOption searchOption = null);
        Task<IGalleryModel> TaggedGallery(string name, int page = 0);
    }
}
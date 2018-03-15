using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Services;

namespace iHentai.Apis.Core
{
    public interface IHentaiApi : IApi
    {
        string Host { get; }

        SearchOptionBase SearchOptionGenerator { get; }

        Task<(int MaxPage, IEnumerable<IGalleryModel> Gallery)> Gallery(IInstanceData data, int page = 0,
            SearchOptionBase searchOption = null, CancellationToken cancellationToken = default);

        Task<IGalleryDetailModel> Detail(IInstanceData data, IGalleryModel model, CancellationToken cancellationToken = default);
    }

    public interface IShareApi
    {
        string GetWebLink(IGalleryModel model);
    }
}
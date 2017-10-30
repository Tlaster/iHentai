using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace iHentai.Core.Common
{
    public interface IIncrementalSource<IType>
    {
        Task<IEnumerable<IType>> GetPagedItemsAsync(int pageIndex, CancellationToken cancellationToken = default);
    }
}
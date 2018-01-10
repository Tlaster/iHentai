using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Collections;

namespace Conet.Apis.Core
{
    public abstract class ConetDataSource<T> : IIncrementalSource<T>
    {
        protected long Curser = default;

        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                Curser = default;
            }

            var data = await GetDataAsync(Curser, pageSize, cancellationToken);
            Curser = data.Curser;
            return data.Data;
        }

        protected abstract Task<(long Curser, IEnumerable<T> Data)> GetDataAsync(long curser, int pageSize, CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Annotations;
using iHentai.Common.Collection;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.Manhuagui
{
    class ManhuaguiUpdateViewModel : TabViewModelBase, IIncrementalSource<ManhuaguiGallery>
    {
        public LoadingCollection<IIncrementalSource<ManhuaguiGallery>, ManhuaguiGallery> Source { get; }

        public ManhuaguiUpdateViewModel()
        {
            Source = new LoadingCollection<IIncrementalSource<ManhuaguiGallery>, ManhuaguiGallery>(this);
        }

        public async Task<IEnumerable<ManhuaguiGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Singleton<ManhuaguiApi>.Instance.Update(pageIndex + 1);
        }
    }
}

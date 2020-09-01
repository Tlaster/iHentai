using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Extensions.Models;
using iHentai.Services;
using iHentai.Services.Models.Core;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;

namespace iHentai.ViewModels.Script
{
    class ScriptGalleryViewModel : ViewModelBase, IIncrementalSource<IGallery>
    {
        public IMangaApi? Api { get; private set; }

        public ScriptGalleryViewModel(ExtensionManifest manifest)
        {
            Manifest = manifest;
            Source = new IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery>(this);
            SwitchTo(manifest);
        }

        public ExtensionManifest Manifest { get; }

        public IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery> Source { get; }

        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Api == null)
            {
                return new List<IGallery>();
            }
            return await Api.Home(pageIndex);
        }

        public async void SwitchTo(ExtensionManifest manifest)
        {
            var api = await HentaiApp.Instance.ExtensionManager.GetApi(manifest);
            if (api != null && api != Api)
            {
                Api = api;
                Source.Clear();
                Source.RefreshAsync();
            }
        }
    }
}

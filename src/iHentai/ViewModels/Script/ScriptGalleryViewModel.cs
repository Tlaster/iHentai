using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Services;
using iHentai.Services.Models.Core;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using PropertyChanged;

namespace iHentai.ViewModels.Script
{
    internal class ScriptGalleryViewModel : ViewModelBase, IIncrementalSource<IGallery>
    {
        private Func<int, Task<IEnumerable<IGallery>>> _loadFunc;

        public ScriptGalleryViewModel(ExtensionManifest manifest)
        {
            Manifest = manifest;
            Source = new IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery>(this);
            SwitchTo(manifest);
        }

        public ScriptApi? Api { get; private set; }

        [DependsOn(nameof(Api))] public bool HasSearch => Api?.HasSearch() ?? false;

        public ExtensionManifest Manifest { get; }

        public IncrementalLoadingCollection<IIncrementalSource<IGallery>, IGallery> Source { get; }

        public async Task<IEnumerable<IGallery>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (Api == null)
            {
                return new List<IGallery>();
            }

            return await _loadFunc.Invoke(pageIndex);
        }

        public async void SwitchTo(ExtensionManifest manifest)
        {
            var api = await this.Resolve<IExtensionManager>().GetApi(manifest);
            if (api != null && api != Api)
            {
                Api = api;
                Reset();
            }
        }

        public void Search(string queryText)
        {
            if (Api == null || !Api.HasSearch())
            {
                return;
            }

            _loadFunc = page => Api.Search(queryText, page);
            Source.Clear();
            Source.RefreshAsync();
        }

        public void Reset()
        {
            if (Api == null)
            {
                return;
            }

            _loadFunc = page => Api.Home(page);
            Source.Clear();
            Source.RefreshAsync();
        }
    }
}
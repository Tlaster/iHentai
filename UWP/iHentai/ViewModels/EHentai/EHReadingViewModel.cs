using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using iHentai.Services.EHentai;
using iHentai.Services.EHentai.Model;
using Microsoft.Toolkit.Uwp.UI;

namespace iHentai.ViewModels.EHentai
{
    internal class EHReadingViewModel : ReadingViewModel
    {
        private readonly EHApi _api;
        private readonly string _url;
        
        public EHReadingViewModel(EHApi api, string url)
        {
            _api = api;
            _url = url;
            Init();
        }

        public EHGalleryDetail Detail { get; private set; }

        private async void Init()
        {
            IsLoading = true;
            Detail = await _api.Detail(_url);
            Title = Detail.Name;
            var list = await Task.WhenAll(Detail.Pages.Select(it => _api.Detail(it)));
            Images = list
                .SelectMany(it =>
                    it.LargeImages ?? it.NormalImages?.OfType<IEHGalleryImage>())
                .Select(it => new EHReadingImage(it, _api)).OfType<IReadingImage>().ToList();
            IsLoading = false;
        }
    }

    internal class EHReadingImage : ReadingImageBase
    {
        private readonly IEHGalleryImage _data;
        private readonly EHApi _api;

        public EHReadingImage(IEHGalleryImage it, EHApi api)
        {
            _data = it;
            _api = api;
        }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            //TODO: check if image already downloaded
            var data = await _api.GetImage(_data.Link, removeCache, token);
            if (removeCache)
            {
                await ImageCache.Instance.RemoveAsync(new[] {new Uri(data.Source)});
            }
            return await ImageCache.Instance.GetFromCacheAsync(new Uri(data.Source), cancellationToken: token);
        }
    }
}
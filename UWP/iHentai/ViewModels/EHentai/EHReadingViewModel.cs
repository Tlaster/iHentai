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

        public EHReadingViewModel(EHApi api, string url, IEHGalleryImage image = null)
        {
            _api = api;
            _url = url;
            Init(image);
        }

        public EHGalleryDetail Detail { get; private set; }

        private async void Init(IEHGalleryImage image)
        {
            IsLoading = true;
            Detail = await _api.Detail(_url);
            Title = Detail.Name;
            var list = await Task.WhenAll(Detail.Pages.Select(it => _api.Detail(it)));
            Images = list
                .SelectMany(it =>
                    it.LargeImages ?? it.NormalImages?.OfType<IEHGalleryImage>())
                .Select((it, index )=> new EHReadingImage(it, _api, index + 1)).OfType<IReadingImage>().ToList();
            var selectedIndex = Images.OfType<EHReadingImage>().ToList().FindIndex(it => it.Data.Link == image?.Link);
            if (selectedIndex != -1)
            {
                SelectedIndex = selectedIndex;
            }
            IsLoading = false;
        }
    }

    internal class EHReadingImage : ReadingImageBase
    {
        private readonly EHApi _api;
        private EHGalleryImage _imageData;

        public EHReadingImage(IEHGalleryImage it, EHApi api, int index)
        {
            Data = it;
            _api = api;
            Index = index;
        }

        public IEHGalleryImage Data { get; }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            if (removeCache)
            {
                await ImageCache.Instance.RemoveAsync(new[] {new Uri(_imageData.Source)});
            }
            //TODO: check if image already downloaded
            _imageData = await _api.GetImage(Data.Link, _imageData?.LoadFailed, removeCache, token);
            return await ImageCache.Instance.GetFromCacheAsync(new Uri(_imageData.Source), cancellationToken: token);
        }
    }
}
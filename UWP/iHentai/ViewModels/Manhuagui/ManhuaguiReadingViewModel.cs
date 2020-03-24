using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using iHentai.Services.Manhuagui;
using iHentai.Services.Manhuagui.Model;
using Microsoft.Toolkit.Helpers;

namespace iHentai.ViewModels.Manhuagui
{
    class ManhuaguiReadingViewModel : ReadingViewModel
    {
        private readonly string _url;
        private readonly ManhuaguiGalleryDetail _detail;
        public ManhuaguiReadingViewModel(string url, ManhuaguiGalleryDetail detail)
        {
            _url = url;
            _detail = detail;
            this.Title = detail.Title;
            Init();
        }

        private async void Init()
        {
            IsLoading = true;
            var images = await Singleton<ManhuaguiApi>.Instance.Images(_url);
            Images = images.Select((it, index) => new ReadingImage(it, index, _detail.Title)).OfType<IReadingImage>().ToList();
            IsLoading = false;
        }
    }

}

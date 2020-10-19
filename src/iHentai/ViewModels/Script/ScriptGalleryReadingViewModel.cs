using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using iHentai.Common;
using iHentai.Common.Helpers;
using iHentai.Data;
using iHentai.Data.Models;
using iHentai.ReadingImages;
using iHentai.Services;
using iHentai.Services.Models.Script;
using Microsoft.Toolkit.Uwp.UI;
using Newtonsoft.Json;

namespace iHentai.ViewModels.Script
{
    internal class ScriptGalleryReadingViewModel : ReadingViewModel
    {
        public ScriptGalleryReadingViewModel(ScriptApi api, ScriptGalleryDetailModel detail) : base(
            detail.Title)
        {
            Api = api;
            Detail = detail;
        }

        public ScriptApi Api { get; set; }

        public ScriptGalleryDetailModel Detail { get; }

        public override void SaveReadingHistory()
        {
            ReadingHistoryDb.Instance.AddOrUpdate(
                Detail.Title,
                Detail.Thumb,
                Detail.Id,
                GalleryType.Script,
                new ScriptGalleryHistoryExtra
                {
                    ExtensionId = Api.Id,
                    Detail = Detail.ToJson(),
                    Progress = SelectedIndex,
                }.ToJson(),
                "ScriptGalleryHistoryExtra"
                );
        }

        protected override int RestoreReadingProgress()
        {
            var item = ReadingHistoryDb.Instance.Source.FirstOrDefault(it =>
                it.GalleryId == Detail.Id && it.GalleryType == GalleryType.Script &&
                it.ExtraInstance is ScriptGalleryHistoryExtra extra && extra.ExtensionId == Api.Id);
            if (item == null)
            {
                return 0;
            }
            else
            {
                return (item.ExtraInstance as ScriptGalleryHistoryExtra)?.Progress ?? 0;
            }
        }

        protected override async Task<IEnumerable<IReadingImage>> InitImages()
        {
            List<string> files;
            if (Detail.Id != null)
            {
                files = await ScriptImageListCache.Instance.GetFromCacheAsync($"{Api.Id}_{Detail.Id}", Task.Run<Stream>(async () =>
                {
                    var result = await Api.GalleryImagePages(Detail);
                    return new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
                }));
            }
            else
            {
                files = await Api.GalleryImagePages(Detail);
            }
            return files.Select((it, index) => new ScriptGalleryReadingImage(it, Api, index + 1)).ToList();
        }
    }

    internal class ScriptGalleryReadingImage : ReadingImageBase
    {
        private readonly string _link;
        private readonly ScriptApi _api;

        public ScriptGalleryReadingImage(string link, ScriptApi api, int index)
        {
            _link = link;
            _api = api;
            Index = index;
        }

        protected override async Task<ImageSource> LoadImage(bool removeCache, CancellationToken token)
        {
            var src = await ScriptGalleryReadingImageCache.Instance.GetFromCacheAsync(_link, Task.Run<Stream>(async () =>
            {
                var result = await _api.GetImageFromImagePage(_link);
                return new MemoryStream(Encoding.UTF8.GetBytes(result));
            }, token), cancellationToken: token);

            if (removeCache)
            {
                await ScriptGalleryReadingImageCache.Instance.RemoveAsync(new[] { _link });
                await ProgressImageCache.Instance.RemoveAsync(new[] { new Uri(src), });
                src = await ScriptGalleryReadingImageCache.Instance.GetFromCacheAsync(_link, Task.Run<Stream>(async () =>
                {
                    var result = await _api.GetImageFromImagePage(_link);
                    return new MemoryStream(Encoding.UTF8.GetBytes(result));
                }, token), cancellationToken: token);
            }

            return await ProgressImageCache.Instance.GetFromCacheAsync(new Uri(src), cancellationToken: token,
                progress: this);
        }
    }
}
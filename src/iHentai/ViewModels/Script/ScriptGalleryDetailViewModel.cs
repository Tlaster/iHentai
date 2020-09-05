using System.Threading.Tasks;
using iHentai.Services;
using iHentai.Services.Models.Core;
using iHentai.Services.Models.Script;
using PropertyChanged;

namespace iHentai.ViewModels.Script
{
    internal class ScriptGalleryDetailViewModel : ViewModelBase
    {
        public ScriptGalleryDetailViewModel(ScriptApi api, ScriptGalleryModel gallery)
        {
            Api = api;
            Gallery = gallery;
            Init();
        }

        public bool IsLoading { get; private set; }
        public ScriptApi Api { get; }
        public ScriptGalleryModel Gallery { get; }
        public ScriptGalleryDetailModel? Detail { get; private set; }

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string GalleryTitle => Gallery?.Title ?? Detail?.Title ?? "";

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string Thumb => Detail?.Thumb ?? Gallery?.Thumb ?? "";

        private async void Init()
        {
            IsLoading = true;
            Detail = await Api.Detail(Gallery);
            IsLoading = false;
        }

        public async Task<bool> CheckCanOpenChapter(IMangaChapter chapter)
        {
            return await Api.CheckCanOpenChapter(chapter);
        }
    }
}
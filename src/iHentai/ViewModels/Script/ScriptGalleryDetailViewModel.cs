using System;
using System.Diagnostics;
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
            Refresh();
        }

        public bool IsLoading { get; private set; }
        public ScriptApi Api { get; }
        public ScriptGalleryModel Gallery { get; }
        public ScriptGalleryDetailModel? Detail { get; private set; }
        public Exception? Error { get; private set; }

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string GalleryTitle => Gallery?.Title ?? Detail?.Title ?? "";

        [DependsOn(nameof(Gallery), nameof(Detail))]
        public string Thumb => Detail?.Thumb ?? Gallery?.Thumb ?? "";

        public async Task Refresh()
        {
            IsLoading = true;
            Error = null;
            try
            {
                Detail = await Api.Detail(Gallery);
            }
            catch (Exception exception)
            {
#if DEBUG
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
#endif
                Error = exception;
            }
            IsLoading = false;
        }

        public async Task<bool> CheckCanOpenChapter(IMangaChapter chapter)
        {
            if (Api.HasCheckCanOpenChapter())
            {
                return await Api.CheckCanOpenChapter(chapter);
            }
            else
            {
                return true;
            }
        }
    }
}
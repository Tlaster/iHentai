using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Services.Models.Core;
using LiteDB;
using Newtonsoft.Json;

namespace iHentai.Data.Models
{
    class ReadingHistoryModel : IGallery
    {
        private object? _extraInstance;
        public long Id { get; set; }
        public string GalleryId { get; set; }
        public string? Title { get; set; }
        public string? Thumb { get; set; }
        public GalleryType GalleryType { get; set; }
        public string? Extra { get; set; }
        public string ExtraType { get; set; }
        public DateTime ReadAt { get; set; }

        public object ExtraInstance
        {
            get
            {
                if (_extraInstance == null)
                {
                    _extraInstance = ExtraType switch
                    {
                        "ScriptGalleryHistoryExtra" => JsonConvert.DeserializeObject<ScriptGalleryHistoryExtra>(Extra),
                        "LocalGalleryHistoryExtra" => JsonConvert.DeserializeObject<LocalGalleryHistoryExtra>(Extra),
                        "ScriptGalleryChapterHistoryExtra" => JsonConvert.DeserializeObject<ScriptGalleryChapterHistoryExtra>(Extra),
                        _ => null,
                    };
                }
                return _extraInstance;
            }
        }
    }

    class LocalGalleryHistoryExtra
    {
        public int Progress { get; set; }
        public string Path { get; set; }
    }

    class ScriptGalleryHistoryExtra
    {
        public int Progress { get; set; }
        public string ExtensionId { get; set; }
        public string GalleryId { get; set; }
    }

    class ScriptGalleryChapterHistoryExtra
    {
        public string ExtensionId { get; set; }
        public List<ScriptChapterHistoryExtra> Chapters { get; set; }
    }

    class ScriptChapterHistoryExtra
    {
        public string ChapterId { get; set; }
        public int Progress { get; set; }
    }

    enum GalleryType
    {
        Local,
        Script,
    }
}

using iHentai.Services.Models.Core;
using Newtonsoft.Json;

namespace iHentai.Services.Models.Script
{
    public class ScriptGalleryModel : IGallery
    {
        [JsonProperty("id")] public string? Id { get; set; }
        [JsonProperty("sub_title")] public string? SubTitle { get; set; }
        [JsonProperty("type")] public string? Type { get; set; }
        [JsonProperty("extra")] public string? Extra { get; set; }
        [JsonProperty("title")] public string? Title { get; set; }
        [JsonProperty("thumb")] public string? Thumb { get; set; }
    }
}
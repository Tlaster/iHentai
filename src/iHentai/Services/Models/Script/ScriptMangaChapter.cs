using iHentai.Services.Models.Core;
using Newtonsoft.Json;

namespace iHentai.Services.Models.Script
{
    public class ScriptMangaChapter : IMangaChapter
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("extra")]
        public string? Extra { get; set; }
    }
}
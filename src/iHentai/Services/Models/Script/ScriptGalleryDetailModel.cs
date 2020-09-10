using System.Collections.Generic;
using iHentai.Services.Models.Core;
using Newtonsoft.Json;

namespace iHentai.Services.Models.Script
{
    public class ScriptGalleryDetailModel : IGalleryDetail
    {
        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("sub_title")] public string? SubTitle { get; set; }

        [JsonProperty("desc")] public string? Desc { get; set; }

        [JsonProperty("rating")] public double Rating { get; set; }

        [JsonProperty("max_rating")] public int MaxRating { get; set; }

        [JsonProperty("has_rating")] public bool HasRating { get; set; }

        [JsonProperty("extra")] public string? Extra { get; set; }

        [JsonProperty("chapters")] public List<ScriptGalleryChapter>? Chapters { get; set; }

        [JsonProperty("tags")] public List<ScriptGalleryTagGroup>? Tags { get; set; }

        [JsonProperty("images")] public List<ScriptGalleryThumb>? Images { get; set; }

        [JsonProperty("meta")] public Dictionary<string, string>? Meta { get; set; }

        [JsonProperty("title")] public string? Title { get; set; }

        [JsonProperty("thumb")] public string? Thumb { get; set; }
    }

    public class ScriptGalleryThumb
    {
        [JsonProperty("source")] public string? Source { get; set; }
        [JsonProperty("text")] public string? Text { get; set; }
        [JsonProperty("crop")] public bool Crop { get; set; }
        [JsonIgnore] public bool NonCrop => !Crop;
        [JsonProperty("offset_x")] public double OffsetX { get; set; }
        [JsonProperty("offset_y")] public double OffsetY { get; set; }
        [JsonProperty("thumb_height")] public int ThumbHeight { get; set; }
        [JsonProperty("thumb_width")] public int ThumbWidth { get; set; }
    }

    public class ScriptGalleryChapter : IMangaChapter
    {
        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("extra")] public string? Extra { get; set; }

        [JsonProperty("title")] public string? Title { get; set; }
    }

    public class ScriptGalleryTagGroup
    {
        [JsonProperty("title")] public string? Title { get; set; }

        [JsonProperty("values")] public List<ScriptGalleryTag>? Values { get; set; }
    }

    public class ScriptGalleryTag
    {
        [JsonProperty("value")] public string? Value { get; set; }

        [JsonProperty("extra")] public string? Extra { get; set; }
    }
}
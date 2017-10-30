using System;
using iHentai.Services.Core.Models.Interfaces;
using Newtonsoft.Json;

namespace iHentai.Services.NHentai.Models
{
    public class GalleryModel : IGalleryModel
    {
        [JsonProperty("upload_date")]
        [JsonConverter(typeof(UploadDateConverter))]
        public DateTime UploadDate { get; set; }

        [JsonProperty("num_favorites")]
        public int NumFavorites { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("title")]
        public TitleModel Title { get; set; }

        [JsonProperty("images")]
        public GalleryImagesModel Images { get; set; }

        [JsonProperty("scanlator")]
        public string Scanlator { get; set; }

        [JsonProperty("tags")]
        public TagModel[] Tags { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("num_pages")]
        public int NumPages { get; set; }
    }

    internal class UploadDateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((long) reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long);
        }
    }
}
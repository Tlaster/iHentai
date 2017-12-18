using System;
using System.Globalization;
using Conet.Apis.Core.Models.Interfaces;
using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class DatetimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((DateTime)value).ToString("ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            return DateTime.TryParseExact(str, "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var res) ? res : DateTime.UtcNow;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }

    public class Status : IStatusModel
    {
        [JsonProperty("created_at")]
        [JsonConverter(typeof(DatetimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("idstr")]
        public string Idstr { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("source_allowclick")]
        public long SourceAllowclick { get; set; }

        [JsonProperty("source_type")]
        public long SourceType { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("favorited")]
        public bool Favorited { get; set; }

        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        [JsonProperty("in_reply_to_status_id")]
        public string InReplyToStatusId { get; set; }

        [JsonProperty("in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }

        [JsonProperty("in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        [JsonProperty("pic_urls")]
        public PicUrl[] PicUrls { get; set; }

        [JsonProperty("geo")]
        public object Geo { get; set; }

        [JsonProperty("is_paid")]
        public bool IsPaid { get; set; }

        [JsonProperty("mblog_vip_type")]
        public long MblogVipType { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("pid")]
        public long? Pid { get; set; }

        [JsonProperty("retweeted_status")]
        public Status RetweetedStatus { get; set; }

        [JsonProperty("annotations")]
        public Annotation[] Annotations { get; set; }

        [JsonProperty("reposts_count")]
        public long RepostsCount { get; set; }

        [JsonProperty("comments_count")]
        public long CommentsCount { get; set; }

        [JsonProperty("attitudes_count")]
        public long AttitudesCount { get; set; }

        [JsonProperty("pending_approval_count")]
        public long PendingApprovalCount { get; set; }

        [JsonProperty("isLongText")]
        public bool IsLongText { get; set; }

        [JsonProperty("mlevel")]
        public long Mlevel { get; set; }

        [JsonProperty("visible")]
        public Visible Visible { get; set; }

        [JsonProperty("biz_feature")]
        public long BizFeature { get; set; }

        [JsonProperty("hasActionTypeCard")]
        public long HasActionTypeCard { get; set; }

        [JsonProperty("darwin_tags")]
        public object[] DarwinTags { get; set; }

        [JsonProperty("hot_weibo_tags")]
        public object[] HotWeiboTags { get; set; }

        [JsonProperty("text_tag_tips")]
        public object[] TextTagTips { get; set; }

        [JsonProperty("rid")]
        public string Rid { get; set; }

        [JsonProperty("userType")]
        public long UserType { get; set; }

        [JsonProperty("more_info_type")]
        public long MoreInfoType { get; set; }

        [JsonProperty("cardid")]
        public string Cardid { get; set; }

        [JsonProperty("positive_recom_flag")]
        public long PositiveRecomFlag { get; set; }

        [JsonProperty("gif_ids")]
        public string GifIds { get; set; }

        [JsonProperty("is_show_bulletin")]
        public long IsShowBulletin { get; set; }

        [JsonProperty("comment_manage_info")]
        public CommentManageInfo CommentManageInfo { get; set; }

        [JsonProperty("textLength")]
        public long? TextLength { get; set; }

        [JsonProperty("thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        [JsonProperty("bmiddle_pic")]
        public string BmiddlePic { get; set; }

        [JsonProperty("original_pic")]
        public string OriginalPic { get; set; }

        [JsonProperty("page_type")]
        public long? PageType { get; set; }
    }
}
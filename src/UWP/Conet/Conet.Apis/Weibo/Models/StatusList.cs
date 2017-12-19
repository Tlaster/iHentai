using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class StatusList
    {
        [JsonProperty("statuses")]
        public Status[] Statuses { get; set; }

        [JsonProperty("advertises")]
        public object[] Advertises { get; set; }

        [JsonProperty("ad")]
        public object[] Ad { get; set; }

        [JsonProperty("hasvisible")]
        public bool Hasvisible { get; set; }

        [JsonProperty("previous_cursor")]
        public long PreviousCursor { get; set; }

        [JsonProperty("next_cursor")]
        public long NextCursor { get; set; }

        [JsonProperty("total_number")]
        public long TotalNumber { get; set; }

        [JsonProperty("interval")]
        public long Interval { get; set; }

        [JsonProperty("uve_blank")]
        public long UveBlank { get; set; }

        [JsonProperty("since_id")]
        public long SinceId { get; set; }

        [JsonProperty("max_id")]
        public long MaxId { get; set; }

        [JsonProperty("has_unread")]
        public long HasUnread { get; set; }
    }
}
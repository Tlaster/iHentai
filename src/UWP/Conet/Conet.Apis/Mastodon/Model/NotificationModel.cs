using System;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class NotificationModel
    {
        public const string NOTIFICATIONTYPE_MENTION = "mention";
        public const string NOTIFICATIONTYPE_REBLOG = "reblog";
        public const string NOTIFICATIONTYPE_FAVOURITE = "favourite";
        public const string NOTIFICATIONTYPE_FOLLOW = "follow";
        /// <summary>
        /// The notification ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// One of: <see cref="NOTIFICATIONTYPE_MENTION"/>, <see cref="NOTIFICATIONTYPE_REBLOG"/>, <see cref="NOTIFICATIONTYPE_FAVOURITE"/>, <see cref="NOTIFICATIONTYPE_FOLLOW"/>
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The time the notification was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The <see cref="AccountModel"/> sending the notification to the user
        /// </summary>
        [JsonProperty("account")]
        public AccountModel Account { get; set; }

        /// <summary>
        /// The <see cref="StatusModel"/> associated with the notification, if applicable
        /// </summary>
        [JsonProperty("status")]
        public StatusModel Status { get; set; }
    }
}

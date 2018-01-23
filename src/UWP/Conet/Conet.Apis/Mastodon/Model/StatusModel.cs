using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model
{
    public class StatusModel
    {
        public const string STATUSVISIBILITY_PUBLIC = "public";
        public const string STATUSVISIBILITY_UNLISTED = "unlisted";
        public const string STATUSVISIBILITY_PRIVATE = "private";
        public const string STATUSVISIBILITY_DIRECT = "direct";

        /// <summary>
        /// The ID of the status
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The time the status was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// null or the ID of the status it replies to
        /// </summary>
        [JsonProperty("in_reply_to_id", NullValueHandling = NullValueHandling.Ignore)]
        public int InReplyToId { get; set; } = -1;

        /// <summary>
        /// null or the ID of the account it replies to
        /// </summary>
        [JsonProperty("in_reply_to_account_id", NullValueHandling = NullValueHandling.Ignore)]
        public int InReplyToAccountId { get; set; } = -1;

        /// <summary>
        /// Whether media attachments should be hidden by default
        /// </summary>
        [JsonProperty("sensitive", NullValueHandling = NullValueHandling.Ignore)]
        public bool Sensitive { get; set; }

        /// <summary>
        /// If not empty, warning text that should be displayed before the actual content
        /// </summary>
        [JsonProperty("spoiler_text")]
        public string SpoilerText { get; set; }

        /// <summary>
        /// One of: <see cref="STATUSVISIBILITY_PUBLIC"/>, <see cref="STATUSVISIBILITY_UNLISTED"/>, <see cref="STATUSVISIBILITY_PRIVATE"/>, <see cref="STATUSVISIBILITY_DIRECT"/>
        /// </summary>
        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        /// <summary>
        /// <see cref="ApplicationModel"/> from which the status was posted
        /// </summary>
        [JsonProperty("application")]
        public ApplicationModel Application { get; set; }

        /// <summary>
        /// The <see cref="AccountModel"/> which posted the status
        /// </summary>
        [JsonProperty("account")]
        public AccountModel Account { get; set; }

        /// <summary>
        /// An array of <see cref="AttachmentModel"/>
        /// </summary>
        [JsonProperty("media_attachments")]
        public List<AttachmentModel> MediaAttachments { get; set; }

        /// <summary>
        /// An array of <see cref="MentionModel"/>
        /// </summary>
        [JsonProperty("mentions")]
        public List<MentionModel> Mentions { get; set; }

        /// <summary>
        /// An array of <see cref="TagModel"/>
        /// </summary>
        [JsonProperty("tags")]
        public List<TagModel> Tags { get; set; }

        /// <summary>
        /// A Fediverse-unique resource ID
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Body of the status; this will contain HTML (remote HTML already sanitized)
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// URL to the status page (can be remote)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The number of reblogs for the status
        /// </summary>
        [JsonProperty("reblogs_count")]
        public int ReblogsCount { get; set; }

        /// <summary>
        /// The number of favourites for the status
        /// </summary>
        [JsonProperty("favourites_count")]
        public int FavouritesCount { get; set; }

        /// <summary>
        /// null or the reblogged <see cref="StatusModel"/>
        /// </summary>
        [JsonProperty("reblog")]
        public StatusModel Reblog { get; set; }

        /// <summary>
        /// Whether the authenticated user has favourited the status
        /// </summary>
        [JsonProperty("favourited", NullValueHandling = NullValueHandling.Ignore)]
        public bool Favourited { get; set; }

        /// <summary>
        /// Whether the authenticated user has reblogged the status
        /// </summary>
        [JsonProperty("reblogged", NullValueHandling = NullValueHandling.Ignore)]
        public bool Reblogged { get; set; }
    }
}

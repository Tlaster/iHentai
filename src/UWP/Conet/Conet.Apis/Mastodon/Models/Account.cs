using System;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Models
{
    public class Account
    {
        [JsonProperty("id")]
        public long Id { get; set; }


        [JsonProperty("username")]
        public string UserName { get; set; }


        [JsonProperty("acct")]
        public string AccountName { get; set; }


        [JsonProperty("display_name")]
        public string DisplayName { get; set; }


        [JsonProperty("locked")]
        public bool Locked { get; set; }


        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }


        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }


        [JsonProperty("following_count")]
        public int FollowingCount { get; set; }


        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }


        [JsonProperty("note")]
        public string Note { get; set; }


        [JsonProperty("url")]
        public string ProfileUrl { get; set; }


        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }


        [JsonProperty("avatar_static")]
        public string StaticAvatarUrl { get; set; }


        [JsonProperty("header")]
        public string HeaderUrl { get; set; }


        [JsonProperty("header_static")]
        public string StaticHeaderUrl { get; set; }
    }
}
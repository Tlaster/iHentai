using System;
using Newtonsoft.Json;

namespace Conet.Apis.Sina.Models
{
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("idstr")]
        public string Idstr { get; set; }

        [JsonProperty("class")]
        public long Class { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("cover_image")]
        public string CoverImage { get; set; }

        [JsonProperty("cover_image_phone")]
        public string CoverImagePhone { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("weihao")]
        public string Weihao { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("followers_count")]
        public long FollowersCount { get; set; }

        [JsonProperty("friends_count")]
        public long FriendsCount { get; set; }

        [JsonProperty("pagefriends_count")]
        public long PagefriendsCount { get; set; }

        [JsonProperty("statuses_count")]
        public long StatusesCount { get; set; }

        [JsonProperty("favourites_count")]
        public long FavouritesCount { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(DatetimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("allow_all_act_msg")]
        public bool AllowAllActMsg { get; set; }

        [JsonProperty("geo_enabled")]
        public bool GeoEnabled { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("verified_type")]
        public long VerifiedType { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("insecurity")]
        public Insecurity Insecurity { get; set; }

        [JsonProperty("ptype")]
        public long Ptype { get; set; }

        [JsonProperty("allow_all_comment")]
        public bool AllowAllComment { get; set; }

        [JsonProperty("avatar_large")]
        public string AvatarLarge { get; set; }

        [JsonProperty("avatar_hd")]
        public string AvatarHd { get; set; }

        [JsonProperty("verified_reason")]
        public string VerifiedReason { get; set; }

        [JsonProperty("verified_trade")]
        public string VerifiedTrade { get; set; }

        [JsonProperty("verified_reason_url")]
        public string VerifiedReasonUrl { get; set; }

        [JsonProperty("verified_source")]
        public string VerifiedSource { get; set; }

        [JsonProperty("verified_source_url")]
        public string VerifiedSourceUrl { get; set; }

        [JsonProperty("verified_state")]
        public long? VerifiedState { get; set; }

        [JsonProperty("verified_level")]
        public long? VerifiedLevel { get; set; }

        [JsonProperty("verified_type_ext")]
        public long? VerifiedTypeExt { get; set; }

        [JsonProperty("has_service_tel")]
        public bool? HasServiceTel { get; set; }

        [JsonProperty("verified_reason_modified")]
        public string VerifiedReasonModified { get; set; }

        [JsonProperty("verified_contact_name")]
        public string VerifiedContactName { get; set; }

        [JsonProperty("verified_contact_email")]
        public string VerifiedContactEmail { get; set; }

        [JsonProperty("verified_contact_mobile")]
        public string VerifiedContactMobile { get; set; }

        [JsonProperty("follow_me")]
        public bool FollowMe { get; set; }

        [JsonProperty("like")]
        public bool Like { get; set; }

        [JsonProperty("like_me")]
        public bool LikeMe { get; set; }

        [JsonProperty("online_status")]
        public long OnlineStatus { get; set; }

        [JsonProperty("bi_followers_count")]
        public long BiFollowersCount { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("star")]
        public long Star { get; set; }

        [JsonProperty("mbtype")]
        public long Mbtype { get; set; }

        [JsonProperty("mbrank")]
        public long Mbrank { get; set; }

        [JsonProperty("block_word")]
        public long BlockWord { get; set; }

        [JsonProperty("block_app")]
        public long BlockApp { get; set; }

        [JsonProperty("credit_score")]
        public long CreditScore { get; set; }

        [JsonProperty("user_ability")]
        public long UserAbility { get; set; }

        [JsonProperty("cardid")]
        public string Cardid { get; set; }

        [JsonProperty("avatargj_id")]
        public string AvatargjId { get; set; }

        [JsonProperty("urank")]
        public long Urank { get; set; }

        [JsonProperty("story_read_state")]
        public long StoryReadState { get; set; }

        [JsonProperty("vclub_member")]
        public long VclubMember { get; set; }

        [JsonProperty("dianping")]
        public string Dianping { get; set; }

        [JsonProperty("unicom_free_pc")]
        public string UnicomFreePc { get; set; }

        [JsonProperty("pay_remind")]
        public long? PayRemind { get; set; }

        [JsonProperty("pay_date")]
        public string PayDate { get; set; }
    }
}
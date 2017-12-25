using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class UnreadModel
    {
        [JsonProperty("cmt")]
        public long Cmt { get; set; }

        [JsonProperty("dm")]
        public long Dm { get; set; }

        [JsonProperty("chat_group_client")]
        public long ChatGroupClient { get; set; }

        [JsonProperty("mention_status")]
        public long MentionStatus { get; set; }

        [JsonProperty("mention_cmt")]
        public long MentionCmt { get; set; }

        [JsonProperty("invite")]
        public long Invite { get; set; }

        [JsonProperty("attitude")]
        public long Attitude { get; set; }

        [JsonProperty("msgbox")]
        public long Msgbox { get; set; }

        [JsonProperty("common_attitude")]
        public long CommonAttitude { get; set; }

        [JsonProperty("page_follower")]
        public long PageFollower { get; set; }

        [JsonProperty("all_mention_status")]
        public long AllMentionStatus { get; set; }

        [JsonProperty("attention_mention_status")]
        public long AttentionMentionStatus { get; set; }

        [JsonProperty("all_mention_cmt")]
        public long AllMentionCmt { get; set; }

        [JsonProperty("attention_mention_cmt")]
        public long AttentionMentionCmt { get; set; }

        [JsonProperty("all_cmt")]
        public long AllCmt { get; set; }

        [JsonProperty("attention_cmt")]
        public long AttentionCmt { get; set; }

        [JsonProperty("attention_follower")]
        public long AttentionFollower { get; set; }

        [JsonProperty("chat_group_notice")]
        public long ChatGroupNotice { get; set; }

        [JsonProperty("comment_attitude")]
        public long CommentAttitude { get; set; }

        [JsonProperty("hot_status")]
        public long HotStatus { get; set; }

        [JsonProperty("chat_group_total")]
        public long ChatGroupTotal { get; set; }

        [JsonProperty("message_flow_aggregate")]
        public long MessageFlowAggregate { get; set; }

        [JsonProperty("message_flow_unaggregate")]
        public long MessageFlowUnaggregate { get; set; }

        [JsonProperty("voip")]
        public long Voip { get; set; }

        [JsonProperty("message_flow_agg_at")]
        public long MessageFlowAggAt { get; set; }

        [JsonProperty("message_flow_agg_repost")]
        public long MessageFlowAggRepost { get; set; }

        [JsonProperty("message_flow_agg_comment")]
        public long MessageFlowAggComment { get; set; }

        [JsonProperty("message_flow_agg_attitude")]
        public long MessageFlowAggAttitude { get; set; }

        [JsonProperty("pc_viedo")]
        public long PcViedo { get; set; }

        [JsonProperty("status_24unread")]
        public long Status24Unread { get; set; }

        [JsonProperty("message_flow_aggr_wild_card")]
        public long MessageFlowAggrWildCard { get; set; }

        [JsonProperty("message_flow_unaggr_wild_card")]
        public long MessageFlowUnaggrWildCard { get; set; }

        [JsonProperty("fans_group_unread")]
        public long FansGroupUnread { get; set; }

        [JsonProperty("message_flow_follow")]
        public long MessageFlowFollow { get; set; }

        [JsonProperty("message_flow_unfollow")]
        public long MessageFlowUnfollow { get; set; }

        [JsonProperty("likeuser")]
        public long Likeuser { get; set; }

        [JsonProperty("addfriends")]
        public long Addfriends { get; set; }

        [JsonProperty("with_dm_group")]
        public long WithDmGroup { get; set; }

        [JsonProperty("with_chat_group")]
        public long WithChatGroup { get; set; }

        [JsonProperty("dm_single")]
        public long DmSingle { get; set; }

        [JsonProperty("dm_group")]
        public long DmGroup { get; set; }

        [JsonProperty("follower")]
        public long Follower { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("ext")]
        public Dictionary<string, long> Ext { get; set; }

        [JsonProperty("ext_new")]
        public ExtNew ExtNew { get; set; }

        [JsonProperty("sys_notice")]
        public long SysNotice { get; set; }

        [JsonProperty("app_message")]
        public AppMessage AppMessage { get; set; }

        [JsonProperty("notice")]
        public long Notice { get; set; }

        [JsonProperty("group")]
        public long Group { get; set; }

        [JsonProperty("photo")]
        public long Photo { get; set; }

        [JsonProperty("badge")]
        public long Badge { get; set; }

        [JsonProperty("tome")]
        public long Tome { get; set; }

        [JsonProperty("bilateral_status")]
        public long BilateralStatus { get; set; }

        [JsonProperty("close_friends_mention_status")]
        public long CloseFriendsMentionStatus { get; set; }

        [JsonProperty("close_friends_mention_cmt")]
        public long CloseFriendsMentionCmt { get; set; }

        [JsonProperty("close_friends_cmt")]
        public long CloseFriendsCmt { get; set; }

        [JsonProperty("friends_suggestions")]
        public long FriendsSuggestions { get; set; }

        [JsonProperty("status_feed")]
        public long StatusFeed { get; set; }

        [JsonProperty("status_hot")]
        public long StatusHot { get; set; }

        [JsonProperty("status_home")]
        public long StatusHome { get; set; }

        [JsonProperty("idc")]
        public string Idc { get; set; }

        [JsonProperty("all_follower")]
        public long AllFollower { get; set; }
    }

    public class ExtNew
    {
        [JsonProperty("follower")]
        public RedPackets Follower { get; set; }

        [JsonProperty("likeuser")]
        public RedPackets Likeuser { get; set; }

        [JsonProperty("addfriends")]
        public RedPackets Addfriends { get; set; }

        [JsonProperty("story_sticker")]
        public CardBackground StorySticker { get; set; }

        [JsonProperty("health_weight")]
        public RedPackets HealthWeight { get; set; }

        [JsonProperty("licai")]
        public RedPackets Licai { get; set; }

        [JsonProperty("jieqian")]
        public RedPackets Jieqian { get; set; }

        [JsonProperty("weiboweika")]
        public RedPackets Weiboweika { get; set; }

        [JsonProperty("notification")]
        public CardBackground Notification { get; set; }

        [JsonProperty("commercial_Service")]
        public RedPackets CommercialService { get; set; }

        [JsonProperty("radar_person")]
        public RedPackets RadarPerson { get; set; }

        [JsonProperty("RedPackets")]
        public RedPackets RedPackets { get; set; }

        [JsonProperty("radar_recom")]
        public RedPackets RadarRecom { get; set; }

        [JsonProperty("health_feed")]
        public CardBackground HealthFeed { get; set; }

        [JsonProperty("radar_didi")]
        public RedPackets RadarDidi { get; set; }

        [JsonProperty("radar_card_coupon")]
        public RedPackets RadarCardCoupon { get; set; }

        [JsonProperty("radar_tv")]
        public RedPackets RadarTv { get; set; }

        [JsonProperty("sign_gift")]
        public RedPackets SignGift { get; set; }

        [JsonProperty("card_background")]
        public CardBackground CardBackground { get; set; }

        [JsonProperty("lucky_treasure")]
        public CardBackground LuckyTreasure { get; set; }

        [JsonProperty("fansService")]
        public RedPackets FansService { get; set; }

        [JsonProperty("story_face_filter")]
        public CardBackground StoryFaceFilter { get; set; }

        [JsonProperty("weibo_level")]
        public CardBackground WeiboLevel { get; set; }

        [JsonProperty("horoscope")]
        public CardBackground Horoscope { get; set; }

        [JsonProperty("health_exercise")]
        public RedPackets HealthExercise { get; set; }

        [JsonProperty("health_checkin")]
        public RedPackets HealthCheckin { get; set; }

        [JsonProperty("huanshouji")]
        public RedPackets Huanshouji { get; set; }

        [JsonProperty("jifen")]
        public CardBackground Jifen { get; set; }

        [JsonProperty("crowdfunding")]
        public CardBackground Crowdfunding { get; set; }

        [JsonProperty("radar_movie")]
        public RedPackets RadarMovie { get; set; }

        [JsonProperty("health_icon")]
        public RedPackets HealthIcon { get; set; }

        [JsonProperty("qiandao")]
        public RedPackets Qiandao { get; set; }

        [JsonProperty("unreadvideo")]
        public RedPackets Unreadvideo { get; set; }
    }

    public class CardBackground
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class RedPackets
    {
        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public class AppMessage
    {
        [JsonProperty("app_unreadcount")]
        public long AppUnreadcount { get; set; }

        [JsonProperty("apps")]
        public object[] Apps { get; set; }
    }
}

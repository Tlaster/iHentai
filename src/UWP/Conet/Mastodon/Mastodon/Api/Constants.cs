namespace Mastodon.Api
{
    internal sealed class Constants
    {
        private const string Version = "/api/v1";

        public const string AccountsFetching = Version + "/accounts/{0}";
        public const string AccountsVerifyCredentials = Version + "/accounts/verify_credentials";
        public const string AccountsUpdateCredentials = Version + "/accounts/update_credentials";
        public const string AccountsFollowers = Version + "/accounts/{0}/followers";
        public const string AccountsFollowing = Version + "/accounts/{0}/following";
        public const string AccountsStatuses = Version + "/accounts/{0}/statuses";
        public const string AccountsFollow = Version + "/accounts/{0}/follow";
        public const string AccountsUnFollow = Version + "/accounts/{0}/unfollow";
        public const string AccountsBlock = Version + "/accounts/{0}/block";
        public const string AccountsUnBlock = Version + "/accounts/{0}/unblock";
        public const string AccountsMute = Version + "/accounts/{0}/mute";
        public const string AccountsUnMute = Version + "/accounts/{0}/unmute";
        public const string AccountsRelationships = Version + "/accounts/relationships";
        public const string AccountsSearch = Version + "/accounts/search";

        public const string AppsRegistering = Version + "/apps";

        public const string BlocksFetching = Version + "/blocks";
        public const string BlocksDomain = Version + "/domain_blocks";

        public const string FavouritesFetching = Version + "/favourites";

        public const string FollowRequestsFetching = Version + "/follow_requests";
        public const string FollowRequestsAuthorize = Version + "/follow_requests/{0}/authorize";
        public const string FollowRequestsReject = Version + "/follow_requests/{0}/reject";

        public const string FollowsFollowing = Version + "/follows";

        public const string Instance = Version + "/instance";

        public const string CustomEmojis = Version + "/custom_emojis";

        public const string List = Version + "/lists";
        public const string ListsByMembership = "/accounts/{0}/lists";
        public const string AccountsInList = "/lists/{0}/accounts";
        public const string ListById = Version + "/lists/{0}";

        public const string MediaUploading = Version + "/media";

        public const string MutesFetching = Version + "/mutes";

        public const string NotificationsFetching = Version + "/notifications";
        public const string NotificationsSingle = Version + "/notifications/{0}";
        public const string NotificationsClear = Version + "/notifications/clear";
        public const string NotificationsDismiss = Version + "/notifications/dismiss";

        public const string ReportsFetching = Version + "/reports";
        public const string ReportsReporting = Version + "/reports";

        public const string Search = Version + "/search";

        public const string StatusesFetching = Version + "/statuses/{0}";
        public const string StatusesContext = Version + "/statuses/{0}/context";
        public const string StatusesCard = Version + "/statuses/{0}/card";
        public const string StatusesRebloggedBy = Version + "/statuses/{0}/reblogged_by";
        public const string StatusesFavouritedBy = Version + "/statuses/{0}/favourited_by";
        public const string StatusesPost = Version + "/statuses";
        public const string StatusesDelete = Version + "/statuses/{0}";
        public const string StatusesReblog = Version + "/statuses/{0}/reblog";
        public const string StatusesUnReblog = Version + "/statuses/{0}/unreblog";
        public const string StatusesFavourite = Version + "/statuses/{0}/favourite";
        public const string StatusesUnFavourite = Version + "/statuses/{0}/unfavourite";
        public const string StatusesPin = Version + "/statuses/{0}/pin";
        public const string StatusesUnpin = Version + "/statuses/{0}/unpin";
        public const string StatusesMute = Version + "/statuses/{0}/mute";
        public const string StatusesUnmute = Version + "/statuses/{0}/unmute";

        public const string TimelineHome = Version + "/timelines/home";
        public const string TimelinePublic = Version + "/timelines/public";
        public const string TimelineTag = Version + "/timelines/tag/{0}";
        public const string TimelineList = Version + "/timelines/list/{0}";

        public const string OAuthAuthorize = "/oauth/authorize";
        public const string OAuthToken = "/oauth/token";

        public const string NoRedirect = "urn:ietf:wg:oauth:2.0:oob";
    }

    internal static class StringExtension
    {
        public static string Id(this string str, string id)
        {
            return string.Format(str, id);
        }
    }
}
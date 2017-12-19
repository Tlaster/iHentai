using Newtonsoft.Json;

namespace Conet.Apis.Weibo.Models
{
    public class CommentManageInfo
    {
        [JsonProperty("comment_permission_type")]
        public long CommentPermissionType { get; set; }

        [JsonProperty("approval_comment_type")]
        public long ApprovalCommentType { get; set; }
    }
}
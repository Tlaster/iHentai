using System.Collections.Generic;

namespace Conet.Apis.Mastodon.Models
{
    public class MastodonList<T> : List<T>
    {
        public long? NextPageMaxId { get; internal set; }
        public long? PreviousPageSinceId { get; internal set; }
    }
}
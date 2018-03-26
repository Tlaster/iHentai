using System.Collections.Generic;

namespace Mastodon.Model
{
    public class MastodonList<T> : List<T>
    {
        public MastodonList()
        {
        }

        public MastodonList(IEnumerable<T> collection) : base(collection)
        {
        }

        public MastodonList(int capacity) : base(capacity)
        {
        }

        public long? MaxId { get; internal set; }
        public long? SinceId { get; internal set; }
    }
}
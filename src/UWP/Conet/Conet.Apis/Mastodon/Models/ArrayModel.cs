using System.Collections.Generic;

namespace Conet.Apis.Mastodon.Models
{
    public class ArrayModel<T>
    {
        public List<T> Result { get; set; }
        public long MaxId { get; set; }
        public long SinceId { get; set; }
    }
}

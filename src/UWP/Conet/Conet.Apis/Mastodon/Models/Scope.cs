using System;

namespace Conet.Apis.Mastodon.Models
{
    [Flags]
    public enum Scope
    {
        Read = 1,
        Write = 2,
        Follow = 4
    }
}
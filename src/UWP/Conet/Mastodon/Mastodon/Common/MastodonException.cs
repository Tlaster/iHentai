using System;

namespace Mastodon.Common
{
    public class MastodonException : Exception
    {
        public MastodonException()
        {
        }

        public MastodonException(string message) : base(message)
        {
        }

        public MastodonException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
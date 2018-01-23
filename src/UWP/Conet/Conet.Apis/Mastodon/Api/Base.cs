namespace Conet.Apis.Mastodon.Api
{
    public abstract class Base
    {

        public string Domain { get; }
        public string AccessToken { get; }

        protected Base(string domain, string accessToken)
        {
            Domain = domain;
            AccessToken = accessToken;
        }

    }
}

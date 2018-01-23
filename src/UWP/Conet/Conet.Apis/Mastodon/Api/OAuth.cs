using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model.OAuth;

namespace Conet.Apis.Mastodon.Api
{
    public partial class OAuth
    {
        /// <summary>
        /// Mastodon instance domain
        /// </summary>
        public string Domain { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public string RedirectUri { get; }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain">Mastodon instance domain</param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret">left it empty or you want to authorize by password</param>
        /// <param name="redirectUri"></param>
        public OAuth(string domain, string clientId, string clientSecret = "", string redirectUri = Constants.NoRedirect)
        {
            Domain = domain;
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
        }

        public string GetGrantUrl() =>
            $"{HttpHelper.HTTPS}{Domain}{Constants.OAuthAuthorize}?client_id={ClientId}&response_type=code&redirect_uri={RedirectUri}";

        public async Task<TokenModel> GetAccessTokenByCode(string code) => await GetAccessTokenByCode(Domain, ClientId, ClientSecret, RedirectUri, code);

        public async Task<TokenModel> GetAccessTokenByPassword(string username, string password) => await GetAccessTokenByPassword(Domain, ClientId, ClientSecret, RedirectUri, username, password);

    }
}

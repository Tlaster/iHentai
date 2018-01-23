using System.Threading.Tasks;
using Conet.Apis.Mastodon.Common;
using Conet.Apis.Mastodon.Model.OAuth;

namespace Conet.Apis.Mastodon.Api
{
    public partial class OAuth
    {
        public static async Task<TokenModel> GetAccessTokenByCode(string domain, string client_id, string client_secret, string redirect_uri, string code, params string[] scope)
        {
            return await HttpHelper.PostAsync<TokenModel, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null, new []
            {
                ( nameof(client_id), client_id ),
                ( nameof(client_secret), client_secret ),
                ( nameof(redirect_uri), redirect_uri ),
                ( "grant_type", "authorization_code" ),
                ( nameof(code), code),
                ( nameof(scope), string.Join(" ", scope) )
            });
        }
        public static async Task<TokenModel> GetAccessTokenByPassword(string domain, string client_id, string client_secret, string redirect_uri, string username, string password, params string[] scope)
        {
            return await HttpHelper.PostAsync<TokenModel, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null, new[]
            {
                ( nameof(client_id), client_id ),
                ( nameof(client_secret), client_secret ),
                ( nameof(redirect_uri), redirect_uri ),
                ( "grant_type", "password" ),
                ( nameof(username), username ),
                ( nameof(password), password ),
                ( nameof(scope), string.Join(" ", scope) )
            });
        }
    }
}

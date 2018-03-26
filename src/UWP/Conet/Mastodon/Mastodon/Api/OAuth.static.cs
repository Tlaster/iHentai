using System.Linq;
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public class OAuth
    {
        public static async Task<Auth> GetAccessTokenByCode(string domain, string client_id, string client_secret,
            string redirect_uri, string code, params Scope[] scope)
        {
            return await HttpHelper.PostAsync<Auth, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null,
                (nameof(client_id), client_id), (nameof(client_secret), client_secret),
                (nameof(redirect_uri), redirect_uri), ("grant_type", "authorization_code"), (nameof(code), code),
                (nameof(scope), string.Join(" ", scope.Select(it => it.ToString("F").ToLowerInvariant()))));
        }

        public static async Task<Auth> GetAccessTokenByPassword(string domain, string client_id, string client_secret,
            string username, string password, params Scope[] scope)
        {
            return await HttpHelper.PostAsync<Auth, string>($"{HttpHelper.HTTPS}{domain}{Constants.OAuthToken}", null,
                (nameof(client_id), client_id), (nameof(client_secret), client_secret),
                ("grant_type", "password"), (nameof(username), username),
                (nameof(password), password), (nameof(scope), string.Join(" ", scope.Select(it => it.ToString("F").ToLowerInvariant()))));
        }
    }
}
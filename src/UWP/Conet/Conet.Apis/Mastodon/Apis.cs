using System;
using System.Collections;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Conet.Apis.Core;
using Conet.Apis.Mastodon.Api;
using iHentai.Services;

namespace Conet.Apis.Mastodon
{
    [ApiKey(nameof(Mastodon))]
    public class Apis : IConetApi
    {
        public ILoginData LoginDataGenerator => new LoginData();

        public async Task<IInstanceData> Login(ILoginData data, CancellationToken token = default)
        {
            if (!(data is LoginData loginData)) throw new ArgumentException();
            if (string.IsNullOrEmpty(loginData.Domain))
                throw new UriFormatException();
            var match = Regex.Match(loginData.Domain, "(http://|https://)?([^/]*)(/)?").Groups[2];
            if (!match.Success)
                throw new UriFormatException();
            var domain = match.Value;
            var auth = await Apps.Register(domain, "Conet", "https://github.com/Tlaster/iHentai",
                "https://github.com/Tlaster/iHentai", Apps.SCOPE_READ, Apps.SCOPE_WRITE, Apps.SCOPE_FOLLOW);
            var url =
                $"https://{domain}/oauth/authorize?client_id={auth.ClientId}&response_type=code&redirect_uri={auth.RedirectUri}&scope={Apps.SCOPE_READ} {Apps.SCOPE_WRITE} {Apps.SCOPE_FOLLOW}";
            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(url),
                new Uri(auth.RedirectUri));
            if (result.ResponseStatus != WebAuthenticationStatus.Success) throw new AuthenticationException();
            var code = Regex.Match(result.ResponseData, "code=(.*)").Groups[1].Value;
            if (string.IsNullOrEmpty(code))
                throw new UnauthorizedAccessException();
            var tokenModel = await OAuth.GetAccessTokenByCode(domain, auth.ClientId, auth.ClientSecret,
                auth.RedirectUri, code, Apps.SCOPE_READ, Apps.SCOPE_WRITE, Apps.SCOPE_FOLLOW);
            var account = await Accounts.VerifyCredentials(domain, tokenModel.AccessToken);
            return new InstanceData(tokenModel, domain, account.Id);
        }

        public Type InstanceDataType { get; } = typeof(InstanceData);

        public async Task<(long Cursor, IEnumerable Data)> HomeTimeline(IInstanceData data, int count = 20,
            long max_id = 0L,
            long since_id = 0L)
        {
            if (!(data is InstanceData model)) throw new ArgumentException();

            var res = await Timelines.Home(model.Domain, model.AccessToken, max_id, since_id);
            return (res.MaxId, res.Result);
        }

        public async Task<object> User(IInstanceData data, long uid)
        {
            if (!(data is InstanceData model)) throw new ArgumentException();

            return await Accounts.Fetching(model.Domain, uid, model.AccessToken);
        }

        public Task<(string Uri, string CallbackUri)> GetOAuth(ILoginData data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OAuthResponseHandler(string response)
        {
            throw new NotImplementedException();
        }
    }
}
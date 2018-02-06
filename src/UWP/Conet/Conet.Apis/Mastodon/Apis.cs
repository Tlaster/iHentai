using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Conet.Apis.Core;
using Conet.Apis.Core.ViewModels;
using Conet.Apis.Mastodon.Api;
using Conet.Apis.Mastodon.Models;
using Conet.Apis.Mastodon.ViewModels;
using iHentai.Basic.Controls;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Services;
using Newtonsoft.Json.Linq;

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
            return new InstanceData(tokenModel.AccessToken, domain, account.Value<string>("id"));
        }

        public Type InstanceDataType { get; } = typeof(InstanceData);

        public async Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20,
            long cursor = 0L)
        {
            if (!(data is InstanceData model)) throw new ArgumentException();

            var res = await Timelines.Home(model.Domain, model.AccessToken, cursor, 0);
            return (res.MaxId, res.Result);
        }

        public async Task<(long Cursor, IEnumerable<JToken> Data)> UserTimeline(IInstanceData data, int count,
            long cursor = 0)
        {
            if (!(data is InstanceData model)) throw new ArgumentException();
            var result = await Accounts.Statuses(model.Domain, model.AccessToken, model.Uid, cursor);
            return (result.MaxId, result.Result);
        }

        public async Task<JToken> Relationship(InstanceData data, string id)
        {
            return (await Accounts.Relationships(data.Domain, data.AccessToken, id)).Result.FirstOrDefault();
        }

        public async Task<ArrayModel<JToken>> Relationships(InstanceData data, params string[] ids)
        {
            return await Accounts.Relationships(data.Domain, data.AccessToken, ids);
        }

        public async Task<JToken> User(IInstanceData data, string uid)
        {
            if (!(data is InstanceData model)) throw new ArgumentException();

            return await Accounts.Fetching(model.Domain, uid, model.AccessToken);
        }

        public IEnumerable<IConetViewModel> GetHomeContent(Guid data, Guid messageGuid)
        {
            yield return new HomeTimelineViewModel(nameof(Mastodon), messageGuid, data);
            yield return new UserTimelineViewModel(data.Get<InstanceData>().Uid, messageGuid, data);
        }
    }

}
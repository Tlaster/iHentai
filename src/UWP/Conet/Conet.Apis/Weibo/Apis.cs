using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Conet.Apis.Core;
using Flurl;
using iHentai.Basic.Helpers;
using iHentai.Services;
using Newtonsoft.Json.Linq;
using Refit;

namespace Conet.Apis.Weibo
{
    [ApiKey(nameof(Weibo))]
    public class Apis : IConetApi
    {
        private readonly IWeiboServices _api;
        private readonly IWeiboServicesV2 _apiv2;

        public Apis()
        {
            _api = RestService.For<IWeiboServices>($"https://{Host}/2", new RefitSettings
            {
                HttpMessageHandlerFactory = () => Singleton<ApiHttpClient>.Instance,
                UrlParameterFormatter = new WeiboParameterFormatter()
            });
            _apiv2 = RestService.For<IWeiboServicesV2>($"https://{HostV2}/2", new RefitSettings
            {
                HttpMessageHandlerFactory = () => Singleton<ApiHttpClient>.Instance,
                UrlParameterFormatter = new WeiboParameterFormatter()
            });
        }

        public string HostV2 { get; } = "api.weibo.com";

        public string Host { get; } = "api.weibo.cn";

        public ILoginData LoginDataGenerator => new LoginData();

        public Type InstanceDataType => typeof(InstanceData);

        public async Task<IInstanceData> Login(ILoginData data, CancellationToken token = default)
        {
            if (!(data is LoginData loginData)) throw new ArgumentException();
            var uri = new Uri(GetOauthLoginPage(loginData));
            var callbackUri = new Uri(loginData.RedirectUri);
            var result =
                await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, uri, callbackUri);
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var uid = long.Parse(Regex.Match(result.ResponseData, "uid=([^\\&]*)").Groups[1].Value);
                var accessToken = Regex.Match(result.ResponseData, "access_token=([^\\&]*)").Groups[1].Value;
                return new InstanceData
                {
                    AccessToken = accessToken,
                    Uid = uid,
                    Source = loginData.AppID
                };
            }

            throw new TaskCanceledException();
        }
        
        public async Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data,
            int count = 20, long cursor = 0L)
        {
            if (!(data is InstanceData instanceData)) throw new ArgumentException();
            var res = await _apiv2.HomeTimeline(instanceData.AccessToken, instanceData.Source, count, cursor);
            return (res.Value<long>("next_cursor"), res.Value<JArray>("statuses"));
        }

        public Task<(long Cursor, IEnumerable<JToken> Data)> UserTimeline(IInstanceData data, int count, long cursor = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<JToken> User(IInstanceData data, string uid)
        {
            if (!(data is InstanceData instanceData)) throw new ArgumentException();
            return await _apiv2.User(instanceData.AccessToken, instanceData.Source, Convert.ToInt64(uid));
        }

        public IEnumerable<IConetViewModel> GetHomeContent()
        {
            throw new NotImplementedException();
        }

        private string GetOauthLoginPage(LoginData data)
        {
            return $@"https://open.weibo.cn/oauth2/authorize?client_id={data.AppID}&response_type=token&redirect_uri={
                    data.RedirectUri
                }&key_hash={data.AppSecret}{
                    (string.IsNullOrEmpty(data.PackageName) ? "" : $"&packagename={data.PackageName}")
                }&display=mobile&scope={data.Scope}";
        }
    }
}
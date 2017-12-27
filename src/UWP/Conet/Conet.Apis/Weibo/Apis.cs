﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Conet.Apis.Core;
using Conet.Apis.Core.Models.Interfaces;
using Flurl;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Services;
using Refit;

namespace Conet.Apis.Weibo
{
    [ApiKey(nameof(ServiceTypes.Weibo))]
    public class Apis : IConetApi, IWithHttpHandler
    {
        private readonly IWeiboServices _api;

        public string Host { get; } = "api.weibo.cn";

        public Apis()
        {
            _api = RestService.For<IWeiboServices>($"https://{Host}/2/", new RefitSettings
            {
                HttpMessageHandlerFactory = () => Singleton<ApiHttpClient>.Instance,
                UrlParameterFormatter = new WeiboParameterFormatter()
            });
        }

        public ILoginData LoginDataGenerator => new LoginData();

        public Task<IEnumerable<IStatusModel>> HomeTimeline(long max_id = 0, long since_id = 0)
        {
            throw new NotImplementedException();
        }

        public Task<(string Uri, string CallbackUri)> GetOAuth(ILoginData data)
        {
            if (!(data is LoginData loginData))
                throw new ArgumentException();
            return Task.FromResult((GetOauthLoginPage(loginData), loginData.RedirectUri));
        }

        public Task<bool> OAuthResponseHandler(string response)
        {
            var regex = Regex.Match(response, "access_token=(.*)\\&remind_in=([0-9]*)");
            var token = regex.Groups[1].Value;
            if (token.IsEmpty())
            {
                throw new UnauthorizedAccessException();
            }
            return Task.FromResult(true);
        }

        public AccountModel Account { get; set; }

        private string GetOauthLoginPage(LoginData data)
            => $@"https://open.weibo.cn/oauth2/authorize?client_id={data.AppID}&response_type=token&redirect_uri={data.RedirectUri}&key_hash={data.AppSecret}{(string.IsNullOrEmpty(data.PackageName) ? "" : $"&packagename={data.PackageName}")}&display=mobile&scope={data.Scope}";

        public bool CanHandle(HttpRequestMessage message)
        {
            return string.Equals(message.RequestUri.Host, Host, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Handle(ref HttpRequestMessage message)
        {
            if (Account != null && Account.Service == ServiceTypes.Weibo)
            {
                message.RequestUri = new Uri($"{message.RequestUri}".SetQueryParam("access_token", Account.AccessToken).SetQueryParam("source", Account.CustomData["source"]));
            }
        }
    }
}
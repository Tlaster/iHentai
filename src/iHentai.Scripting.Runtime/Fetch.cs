﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace iHentai.Scripting.Runtime
{
    public sealed class FetchInit
    {
        public string? method { get; set; }
        public string? referrer { get; set; }
        public IDictionary<string, string>? headers { get; set; }
        public string? bodyType { get; set; }
        public IDictionary<string, string>? body { get; set; }
    }

    public interface IScriptHttpInterceptor
    {
        void Handle(IDisposable message);
    }

    internal class ScriptHttpHandler : HttpClientHandler
    {
        private readonly IScriptHttpInterceptor _interceptor;

        public ScriptHttpHandler(IScriptHttpInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _interceptor.Handle(request);
            return base.SendAsync(request, cancellationToken);
        }
    }

    public sealed class Fetch
    {
        private readonly HttpClient _client;

        public Fetch(IScriptHttpInterceptor interceptor)
        {
            _client = new HttpClient(new ScriptHttpHandler(interceptor));
        }

        public IAsyncOperation<FetchResponse> fetch(string input, FetchInit? init)
        {
            var tsc = new TaskCompletionSource<FetchResponse>();

            Task.Run(async () =>
            {
                var uri = new Uri(input);
                //if (_manifest.Hosts == null || !_manifest.Hosts.Contains(uri.Host))
                //{
                //    tsc.SetException(new Exception("Access denied for target uri"));
                //    return;
                //}

                if (init == null)
                {
                    using var clientResponse = await _client.GetAsync(uri);
                    tsc.SetResult(await FetchResponse.FromHttpResponseMessage(clientResponse));
                    return;
                }

                using var requestMessage = new HttpRequestMessage { RequestUri = uri };
                if (!string.IsNullOrEmpty(init.method))
                {
                    requestMessage.Method = new HttpMethod(init.method);
                }

                if (init.headers != null)
                {
                    foreach (var (key, value) in init.headers)
                    {
                        requestMessage.Headers.Add(key, value);
                    }
                }

                if (init.body != null)
                {
                    if (!string.IsNullOrEmpty(init.bodyType))
                    {
                        HttpContent? content = init.bodyType switch
                        {
                            "UrlEncoded" => new FormUrlEncodedContent(init.body),
                            _ => null
                        };
                        if (content != null)
                        {
                            requestMessage.Content = content;
                        }
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(string.Join(";", init.body.Select(it => it.Key + "=" + it.Value)), Encoding.UTF8);
                    }
                }


                if (!string.IsNullOrEmpty(init.referrer))
                {
                    requestMessage.Headers.Referrer = new Uri(init.referrer);
                }

                using var response = await _client.SendAsync(requestMessage);
                tsc.SetResult(await FetchResponse.FromHttpResponseMessage(response));
            });

            return tsc.Task.AsAsyncOperation();
        }
    }

    public sealed class FetchHeader
    {
        private readonly IDictionary<string, string> _headers;

        public FetchHeader(IDictionary<string, string> headers)
        {
            _headers = headers;
        }

        public string? get(string name)
        {
            if (_headers.ContainsKey(name))
            {
                return _headers[name];
            }
            else
            {
                return null;
            }
        }

        public bool has(string name)
        {
            return _headers.ContainsKey(name);
        }

    }

    public sealed class FetchResponse
    {
        public FetchResponse(IDictionary<string, string> headers, bool ok, bool redirected, int status,
            string statusText, string type, string url, string body, string bodyType)
        {
            this.headers = new FetchHeader(headers);
            this.ok = ok;
            this.redirected = redirected;
            this.status = status;
            this.statusText = statusText;
            this.type = type;
            this.url = url;
            this.body = body;
            this.bodyType = bodyType;
        }

        public FetchHeader headers { get; }
        public bool ok { get; }

        public bool redirected { get; }

        public int status { get; }
        public string statusText { get; }
        public string type { get; }
        public string url { get; }
        public string body { get; }
        public string bodyType { get; }

        internal static async Task<FetchResponse> FromHttpResponseMessage(HttpResponseMessage message)
        {
            return new FetchResponse(
                message.Headers.ToDictionary(x => x.Key, x =>
                {
                    if (x.Key == "Set-Cookie")
                    {
                        return string.Join(";", x.Value.Select(item => item.Split(';').FirstOrDefault()));
                    }
                    return string.Join("", x.Value);
                }),
                message.IsSuccessStatusCode, false, (int)message.StatusCode,
                message.StatusCode.ToString(), "basic", message.RequestMessage.RequestUri.ToString(),
                await message.Content.ReadAsStringAsync(), "string");
        }

        public IAsyncOperation<string> text()
        {
            return Task.FromResult(body).AsAsyncOperation();
        }
    }
}
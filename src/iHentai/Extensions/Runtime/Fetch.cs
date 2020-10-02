using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iHentai.Extensions.Common;
using iHentai.Extensions.Models;
using Jint.Native;
using Newtonsoft.Json;

namespace iHentai.Extensions.Runtime
{
    public class Fetch
    {
        private readonly HttpClient _client;
        private readonly ExtensionManifest _manifest;

        public Fetch(ExtensionManifest manifest, HttpMessageHandler handler)
        {
            _manifest = manifest;
            _client = new HttpClient(handler);
        }

        public FetchResponse fetch(string input, FetchInit? init = null)
        {
            var task = Task.Run(async () =>
            {
                var uri = new Uri(input);
                if (_manifest.Hosts == null || !_manifest.MatchHost(uri))
                {
                    throw new Exception("Access denied for target uri");
                }

                if (init == null)
                {
                    using var clientResponse = await _client.GetAsync(uri);
                    return await FetchResponse.FromHttpResponseMessage(clientResponse);
                }

                using var requestMessage = new HttpRequestMessage { RequestUri = uri };
                if (!string.IsNullOrEmpty(init.Method))
                {
                    requestMessage.Method = new HttpMethod(init.Method);
                }

                if (init.Headers != null)
                {
                    foreach (var (key, value) in init.Headers)
                    {
                        requestMessage.Headers.Add(key, value);
                    }
                }

                if (init.Body != null)
                {
                    if (!string.IsNullOrEmpty(init.BodyType))
                    {
                        HttpContent? content = init.BodyType switch
                        {
                            "UrlEncoded" => new FormUrlEncodedContent(init.Body),
                            _ => null
                        };
                        if (content != null)
                        {
                            requestMessage.Content = content;
                        }
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(string.Join(";", init.Body.Select(it => it.Key + "=" + it.Value)), Encoding.UTF8);
                    }
                }


                if (!string.IsNullOrEmpty(init.Referrer))
                {
                    requestMessage.Headers.Referrer = new Uri(init.Referrer);
                }

                using var response = await _client.SendAsync(requestMessage);
                return await FetchResponse.FromHttpResponseMessage(response);
            });
            task.Wait();
            return task.Result;
        }
    }

    public class FetchHeader
    {
        private readonly Dictionary<string, string> _headers;

        public FetchHeader(Dictionary<string, string> headers)
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

    public class FetchResponse
    {
        public FetchResponse(Dictionary<string, string> headers, bool ok, bool redirected, int status,
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

        public static async Task<FetchResponse> FromHttpResponseMessage(HttpResponseMessage message)
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
                message.IsSuccessStatusCode, false, (int) message.StatusCode,
                message.StatusCode.ToString(), "basic", message.RequestMessage.RequestUri.ToString(),
                await message.Content.ReadAsStringAsync(), "string");
        }

        public string text()
        {
            return body;
        }
    }
    public class FetchInit
    {
        [JsonProperty("method")]
        public string? Method { get; set; }
        [JsonProperty("referrer")]
        public string? Referrer { get; set; }
        [JsonProperty("headers")]
        public Dictionary<string, string>? Headers { get; set; }
        [JsonProperty("bodyType")]
        public string? BodyType { get; set; }
        [JsonProperty("body")]
        public Dictionary<string, string>? Body { get; set; }
    }
}
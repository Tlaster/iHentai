//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using iHentai.Extensions.Common;
//using iHentai.Extensions.Models;
//using NiL.JS.BaseLibrary;
//using NiL.JS.Core;
//using NiL.JS.Core.Interop;
//using NiL.JS.Extensions;

//namespace iHentai.Extensions.Runtime
//{
//    public class Fetch
//    {
//        private readonly HttpClient _client;
//        private readonly ExtensionManifest _manifest;

//        public Fetch(ExtensionManifest manifest, HttpMessageHandler handler)
//        {
//            _manifest = manifest;
//            _client = new HttpClient(handler);
//        }

//        [DoNotDelete]
//        public Task<JSValue> fetch(string input, JSObject? init = null)
//        {
//            var tsc = new TaskCompletionSource<JSValue>();

//            Task.Run(async () =>
//            {
//                var uri = new Uri(input);
//                if (_manifest.Hosts == null || !_manifest.Hosts.Contains(uri.Host))
//                {
//                    tsc.SetException(new JSException(new Error("Access denied for target uri")));
//                    return;
//                }

//                if (init == null)
//                {
//                    using var clientResponse = await _client.GetAsync(uri);
//                    tsc.SetResult(JSValue.Marshal(await FetchResponse.FromHttpResponseMessage(clientResponse)));
//                    return;
//                }

//                using var requestMessage = new HttpRequestMessage {RequestUri = uri};
//                if (init.TryGet("method", out string method))
//                {
//                    requestMessage.Method = new HttpMethod(method);
//                }

//                if (init.TryGet("headers", out JSObject headers))
//                {
//                    foreach (var item in headers)
//                    {
//                        requestMessage.Headers.Add(item.Key, item.Value.As<string>());
//                    }
//                }

//                if (init.TryGet("body", out JSValue body))
//                {
//                    if (init.TryGet("bodyType", out string bodyType))
//                    {
//                        MultipartFormDataContent BuildMultipartFormDataContent()
//                        {
//                            var content = new MultipartFormDataContent();
//                            foreach (var item in body)
//                            {
//                                if (item.Value.Is(JSValueType.Object))
//                                {
//                                    if (item.Value.TryGet("fileName", out string fileName))
//                                    {
//                                        content.Add(new ByteArrayContent(
//                                                Convert.FromBase64String(body.GetProperty("value").ToString()))
//                                            , item.Key, fileName);
//                                    }
//                                    else
//                                    {
//                                        content.Add(new ByteArrayContent(
//                                                Convert.FromBase64String(body.GetProperty("value").ToString()))
//                                            , item.Key);
//                                    }
//                                }
//                                else
//                                {
//                                    content.Add(new StringContent(item.Value.ToString(), Encoding.UTF8), item.Key);
//                                }
//                            }

//                            return content;
//                        }

//                        HttpContent? content = bodyType switch
//                        {
//                            "FormData" => BuildMultipartFormDataContent(),
//                            "UrlEncoded" => new FormUrlEncodedContent(body.ToDictionary(it => it.Key,
//                                it => it.Value.ToString())),
//                            _ => null
//                        };
//                        if (content != null)
//                        {
//                            requestMessage.Content = content;
//                        }
//                    }
//                    else
//                    {
//                        requestMessage.Content = new StringContent(body.ToString(), Encoding.UTF8);
//                    }
//                }

//                if (init.TryGet("referrer", out string referrer))
//                {
//                    requestMessage.Headers.Referrer = new Uri(referrer);
//                }

//                using var response = await _client.SendAsync(requestMessage);
//                tsc.SetResult(JSValue.Marshal(await FetchResponse.FromHttpResponseMessage(response)));
//            });

//            return tsc.Task;
//        }
//    }

//    public class FetchHeader
//    {
//        private readonly Dictionary<string, string> _headers;

//        public FetchHeader(Dictionary<string, string> headers)
//        {
//            _headers = headers;
//        }

//        public string? get(string name)
//        {
//            if (_headers.ContainsKey(name))
//            {
//                return _headers[name];
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public bool has(string name)
//        {
//            return _headers.ContainsKey(name);
//        }

//    }

//    public class FetchResponse
//    {
//        public FetchResponse(Dictionary<string, string> headers, bool ok, bool redirected, int status,
//            string statusText, string type, string url, string body, string bodyType)
//        {
//            this.headers = new FetchHeader(headers);
//            this.ok = ok;
//            this.redirected = redirected;
//            this.status = status;
//            this.statusText = statusText;
//            this.type = type;
//            this.url = url;
//            this.body = body;
//            this.bodyType = bodyType;
//        }

//        public FetchHeader headers { get; }
//        public bool ok { get; }

//        [Hidden] public bool redirected { get; }

//        public int status { get; }
//        public string statusText { get; }
//        public string type { get; }
//        public string url { get; }
//        public string body { get; }
//        public string bodyType { get; }

//        public static async Task<FetchResponse> FromHttpResponseMessage(HttpResponseMessage message)
//        {
//            return new FetchResponse(
//                message.Headers.ToDictionary(x => x.Key, x =>
//                {
//                    if (x.Key == "Set-Cookie")
//                    {
//                        return string.Join(";", x.Value.Select(item => item.Split(';').FirstOrDefault()));
//                    }
//                    return string.Join("", x.Value);
//                }),
//                message.IsSuccessStatusCode, false, (int) message.StatusCode,
//                message.StatusCode.ToString(), "basic", message.RequestMessage.RequestUri.ToString(),
//                await message.Content.ReadAsStringAsync(), "string");
//        }

//        [DoNotDelete]
//        public Task<string> text()
//        {
//            return Task.FromResult(body);
//        }

//        [DoNotDelete]
//        public Promise json()
//        {
//            return Promise.resolve(JSON.parse(body));
//        }
//    }
//}
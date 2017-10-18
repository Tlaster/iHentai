﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Html2Model;

namespace iHentai.Services.Core
{
    internal static class FlurlExtension
    {
        public static async Task<T> ReceiveHtml<T>(this Task<HttpResponseMessage> response) where T: class, new()
        {
            return HtmlConvert.DeserializeObject<T>(await response.ReceiveString());
        }

        public static async Task<object> ReceiveHtml(this Task<HttpResponseMessage> response, Type type)
        {
            return HtmlConvert.DeserializeObject(await response.ReceiveString(), type);
        }
    }
}
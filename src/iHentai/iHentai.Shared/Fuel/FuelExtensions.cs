using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace iHentai.Core.iHentai.Shared.Fuel
{
    public static class FuelExtensions
    {

        public static FuelRequest<T> With<T>(this string url, IDictionary<string, T> data)
        {
            return new FuelRequest<T>(url, data);
        }

        public static FuelRequest<T> AddCookie<T>(this FuelRequest<T> request, string key, string value)
        {
            request.Cookie = request.Cookie ?? new Dictionary<string, string>();
            request.Cookie.Add(key, value);
            return request;
        }
        
        public static FuelRequest<T> Cookie<T>(this FuelRequest<T> request, IDictionary<string, string> cookie)
        {
            request.Cookie = cookie;
            return request;
        }

        public static FuelRequest<T> AddHeader<T>(this FuelRequest<T> request, string key, string value)
        {
            request.Header = request.Header ?? new Dictionary<string, string>();
            request.Header.Add(key, value);
            return request;
        }

        public static FuelRequest<T> Header<T>(this FuelRequest<T> request, IDictionary<string, string> header)
        {
            request.Header = header;
            return request;
        }

        public static FuelRequest<T> Method<T>(this FuelRequest<T> request, HttpMethod method)
        {
            request.Method = method;
            return request;
        }

        public static FuelRequest<T> AsFormUrlEncodedContent<T>(this FuelRequest<T> request)
        {
            request.PostType = PostTypes.FormUrlEncoded;
            return request;
        }
        
        public static FuelRequest<T> AsMultipartFormDataContent<T>(this FuelRequest<T> request)
        {
            request.PostType = PostTypes.MultipartFormData;
            return request;
        }
        
        public static string Execute<T>(this FuelRequest<T> request)
        {
            var req = WebRequest.CreateHttp(request.Uri);
            req.Method = request.Method.ToString();
            if (request.Header != null && request.Header.Any())
            {
                req.Headers = req.Headers ?? new WebHeaderCollection();
                foreach (var item in request.Header)
                {
                    req.Headers.Add(item.Key, item.Value);
                }
            }

            if (request.Cookie != null && request.Cookie.Any())
            {
                req.CookieContainer = req.CookieContainer ?? new CookieContainer();
                foreach (var item in request.Cookie)
                {
                    req.CookieContainer.Add(new Cookie(item.Key, item.Value) {Domain = request.Uri});
                }
            }
        }
    }
}
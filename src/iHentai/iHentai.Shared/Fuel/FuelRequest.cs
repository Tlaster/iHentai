using System.Collections.Generic;
using System.Net.Http;

namespace iHentai.Core.iHentai.Shared.Fuel
{
    public class FuelRequest<T>
    {

        public FuelRequest(string uri, IDictionary<string, T> data)
        {
            Uri = uri;
            Data = data;
        }

        public string Uri { get; }

        public HttpMethod Method { get; set; }

        public IDictionary<string, T> Data { get; set; }

        public IDictionary<string, string> Cookie { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Header { get; set; } = new Dictionary<string, string>();

        public PostTypes PostType { get; set; }
    }
    public enum PostTypes
    {
        MultipartFormData,
        FormUrlEncoded
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Jint;
using LZStringCSharp;

namespace iHentai.Scripting.Runtime
{
    public sealed class RootRuntime
    {
        //private readonly Fetch _fetch;

        //public RootRuntime(IScriptHttpInterceptor interceptor)
        //{
        //    _fetch = new Fetch(interceptor);
        //}

        public HtmlElement parseHtml(string content)
        {
            return HtmlElement.parse(content);
        }

        public string? unpack(string script)
        {
            var engine = new Engine();
            var result = engine.Execute(script).GetCompletionValue().ToObject()?.ToString();
            return result;
        }

        public string decodeLzStringFromBase64(string content)
        {
            return LZString.DecompressFromBase64(content);
        }

        //public IAsyncOperation<FetchResponse> fetch(string input)
        //{
        //    return _fetch.fetch(input, null);
        //}

        //public IAsyncOperation<FetchResponse> fetch(string input, FetchInit init)
        //{
        //    return _fetch.fetch(input, null);
        //}
    }
}

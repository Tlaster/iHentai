using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Services.Models.Core;
using iHentai.Services.Models.Script;
using Jint.Native;
using Newtonsoft.Json;

namespace iHentai.Services
{
    public class ScriptApi : IMangaApi, ISearchableApi, IDetailedApi, ICustomHttpHandler
    {
        private readonly ScriptEngine _engine;
        private readonly string _extensionId;
        private readonly ExtensionManifest _manifest;

        public ScriptApi(ScriptEngine engine, string extensionId, ExtensionManifest manifest)
        {
            _engine = engine;
            _extensionId = extensionId;
            _manifest = manifest;
        }


        public async Task<bool> CanHandle(Uri uri)
        {
            const string functionName = "canModifyRequest";
            if (_manifest.Hosts != null && MatchHost(uri) && _engine.HasMember(functionName))
            {
                return await InvokeAsync<bool>(functionName, uri.ToString());
            }

            return false;
        }

        public async Task Handle(HttpRequestMessage message)
        {
            const string functionName = "modifyRequest";
            if (_engine.HasMember(functionName))
            {
                var arg = Json(ScriptRequestContent.FromHttpRequestMessage(message));
                var content =  await InvokeAsync<ScriptRequestContent>(functionName, arg);
                content.ToHttpRequestMessage(message);
            }
        }

        public bool RequireLogin()
        {
            const string functionName = "requireLogin";
            if (!_engine.HasMember(functionName))
            {
                return false;
            }

            return Invoke<bool>(functionName);
        }

        public async Task<int> Login(string userName, string password)
        {
            const string functionName = "login";
            return await InvokeAsync<int>(functionName,
                userName, password);
        }

        async Task<IGalleryDetail> IDetailedApi.Detail(IGallery gallery)
        {
            return await Detail(gallery);
        }

        public bool HasCheckCanOpenChapter()
        {
            const string functionName = "canReadChapter";
            return _engine.HasMember(functionName);
        }

        public async Task<bool> CheckCanOpenChapter(IMangaChapter chapter)
        {
            const string functionName = "canReadChapter";
            return await InvokeAsync<bool>(functionName, Json(chapter));
        }

        public async Task<List<string>> ChapterImages(IMangaChapter chapter)
        {
            const string functionName = "loadChapterImages";
            return await InvokeAsync<List<string>>(functionName,
                Json(chapter));
        }

        public async Task<List<string>> GalleryImagePages(IGalleryDetail detail)
        {
            const string functionName = "loadGalleryImagePages";
            return await InvokeAsync<List<string>>(functionName,
                Json(detail));
        }

        public async Task<string> GetImageFromImagePage(string page)
        {
            const string functionName = "loadImageFromPage";
            return await InvokeAsync<string>(functionName, page);
        }

        public async Task<IEnumerable<IGallery>> Home(int page)
        {
            const string functionName = "home";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, page);
        }

        public async Task<IEnumerable<IGallery>> Search(string keyword, int page)
        {
            const string functionName = "search";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, keyword, page);
        }

        public async Task<IEnumerable<IGallery>> UserTimeline(int page)
        {
            const string functionName = "userTimeline";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, page);
        }

        public bool HasSearch()
        {
            const string functionName = "search";
            return _engine.HasMember(functionName);
        }

        public bool HasDetail()
        {
            const string functionName = "detail";
            return _engine.HasMember(functionName);
        }

        public bool HasGalleryImages()
        {
            const string functionName = "loadGalleryImages";
            return _engine.HasMember(functionName);
        }

        public async Task<ScriptGalleryDetailModel> Detail(IGallery gallery)
        {
            const string functionName = "detail";
            return await InvokeAsync<ScriptGalleryDetailModel>(functionName,
                Json(gallery));
        }

        private bool MatchHost(Uri uri)
        {
            if (_manifest.Hosts == null)
            {
                return false;
            }

            foreach (var item in _manifest.Hosts)
            {
                if (item == null)
                {
                    continue;
                }

                if (item == uri.Host)
                {
                    return true;
                }

                if (Regex.IsMatch(uri.Host, item))
                {
                    return true;
                }
            }

            return false;
        }

        private JsValue Json(object obj)
        {
            return _engine.JSON.Parse(_engine.Engine.Global,
                new[] {JsValue.FromObject(_engine.Engine, JsonConvert.SerializeObject(obj)),});
        }

        private T Invoke<T>(string name, params object[] arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = _engine.InvokeFunction<string>(name, arguments);
                return JsonConvert.DeserializeObject<T>(scriptResult);
            }

            throw new NotImplementedException();
        }

        private async Task<T> InvokeAsync<T>(string name, params object[] arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = await _engine.InvokeFunctionAsync<string>(name, arguments);
                return JsonConvert.DeserializeObject<T>(scriptResult);
            }

            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iHentai.Common;
using iHentai.Extensions;
using iHentai.Extensions.Models;
using iHentai.Services.Models.Core;
using iHentai.Services.Models.Script;
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
            if (_manifest.Hosts != null && MatchHost(uri) && await _engine.HasMember(functionName))
            {
                return await _engine.InvokeFunction<bool>(functionName, uri.ToString());
            }

            return false;
        }

        public async Task Handle(HttpRequestMessage message)
        {
            const string functionName = "modifyRequest";
            if (await _engine.HasMember(functionName))
            {
                var arg = _engine.JSON.Parse(JsonConvert.SerializeObject(ScriptRequestContent.FromHttpRequestMessage(message)));
                var content = await Invoke<ScriptRequestContent>(functionName, arg);
                content.ToHttpRequestMessage(message);
            }
        }

        public async Task<bool> RequireLogin()
        {
            const string functionName = "requireLogin";
            if (!await _engine.HasMember(functionName))
            {
                return false;
            }

            return await Invoke<bool>(functionName);
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

        public async Task<bool> HasCheckCanOpenChapter()
        {
            const string functionName = "canReadChapter";
            return await _engine.HasMember(functionName);
        }

        public async Task<bool> CheckCanOpenChapter(IMangaChapter chapter)
        {
            const string functionName = "canReadChapter";
            return await InvokeAsync<bool>(functionName,
                _engine.JSON.Parse(JsonConvert.SerializeObject(chapter)));
        }

        public async Task<List<string>> ChapterImages(IMangaChapter chapter)
        {
            const string functionName = "loadChapterImages";
            return await InvokeAsync<List<string>>(functionName,
                _engine.JSON.Parse(JsonConvert.SerializeObject(chapter)));
        }

        public async Task<List<string>> GalleryImagePages(IGalleryDetail detail)
        {
            const string functionName = "loadGalleryImagePages";
            return await InvokeAsync<List<string>>(functionName,
                _engine.JSON.Parse(JsonConvert.SerializeObject(detail)));
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

        public async Task<bool> HasSearch()
        {
            const string functionName = "search";
            return await _engine.HasMember(functionName);
        }

        public async Task<bool> HasDetail()
        {
            const string functionName = "detail";
            return await _engine.HasMember(functionName);
        }

        public async Task<bool> HasGalleryImages()
        {
            const string functionName = "loadGalleryImages";
            return await _engine.HasMember(functionName);
        }

        public async Task<ScriptGalleryDetailModel> Detail(IGallery gallery)
        {
            const string functionName = "detail";
            return await InvokeAsync<ScriptGalleryDetailModel>(functionName,
                _engine.JSON.Parse(JsonConvert.SerializeObject(gallery)));
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

        private async Task<T> Invoke<T>(string name, params object[] arguments)
        {
            if (await _engine.HasMember(name))
            {
                var scriptResult = await _engine.InvokeFunction<string>(name, arguments);
                return JsonConvert.DeserializeObject<T>(scriptResult);
            }

            throw new NotImplementedException();
        }

        private async Task<T> InvokeAsync<T>(string name, params object[] arguments)
        {
            if (await _engine.HasMember(name))
            {
                var scriptResult = await _engine.InvokeFunctionAsync<string>(name, arguments);
                return JsonConvert.DeserializeObject<T>(scriptResult);
            }

            throw new NotImplementedException();
        }
    }
}
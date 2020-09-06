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
using NiL.JS.BaseLibrary;
using NiL.JS.Core;

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


        //public ReadingViewModel? GenerateReadingViewModel(IGallery gallery)
        //{
        //    throw new NotImplementedException();
        //}

        public bool CanHandle(Uri uri)
        {
            const string functionName = "canModifyRequest";
            if (_manifest.Hosts != null && MatchHost(uri) && _engine.HasMember(functionName))
            {
                return _engine.InvokeFunction<bool>(functionName, new Arguments {uri.ToString()});
            }

            return false;
        }

        public void Handle(HttpRequestMessage message)
        {
            const string functionName = "modifyRequest";
            if (_engine.HasMember(functionName))
            {
                var arg = JSON.parse(JsonConvert.SerializeObject(ScriptRequestContent.FromHttpRequestMessage(message)));
                var content = Invoke<ScriptRequestContent>(functionName,
                    new Arguments
                    {
                        arg
                    });
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

            return Invoke<bool>(functionName, new Arguments());
        }

        public async Task<int> Login(string userName, string password)
        {
            const string functionName = "login";
            return await InvokeAsync<int>(functionName,
                new Arguments {userName, password});
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
            return await InvokeAsync<bool>(functionName,
                new Arguments {JSON.parse(JsonConvert.SerializeObject(chapter))});
        }

        //public ReadingViewModel? GenerateReadingViewModel(IGalleryDetail detail, IMangaChapter gallery)
        //{
        //    if (gallery is ScriptMangaChapter item && detail is ScriptGalleryDetailModel detailModel)
        //    {
        //        return new ScriptReadingViewModel(detailModel, item, this);
        //    }
        //    else
        //    {
        //        throw new ArgumentException();
        //    }
        //}

        public async Task<List<string>> ChapterImages(IMangaChapter chapter)
        {
            const string functionName = "loadChapterImages";
            return await InvokeAsync<List<string>>(functionName,
                new Arguments {JSON.parse(JsonConvert.SerializeObject(chapter))});
        }

        public async Task<List<string>> GalleryImages(IGalleryDetail detail)
        {
            const string functionName = "loadGalleryImages";
            return await InvokeAsync<List<string>>(functionName,
                new Arguments {JSON.parse(JsonConvert.SerializeObject(detail))});
        }

        public async Task<IEnumerable<IGallery>> Home(int page)
        {
            const string functionName = "home";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {page});
        }

        public async Task<IEnumerable<IGallery>> Search(string keyword, int page)
        {
            const string functionName = "search";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {keyword, page});
        }

        public async Task<IEnumerable<IGallery>> UserTimeline(int page)
        {
            const string functionName = "userTimeline";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {page});
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
                new Arguments {JSON.parse(JsonConvert.SerializeObject(gallery))});
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

        private T Invoke<T>(string name, Arguments arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = _engine.InvokeFunction<JSValue>(name, arguments);
                var scriptResultText = JSON.stringify(scriptResult);
                return JsonConvert.DeserializeObject<T>(scriptResultText);
            }

            throw new NotImplementedException();
        }

        private async Task<T> InvokeAsync<T>(string name, Arguments arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = await _engine.InvokeFunctionAsync<JSValue>(name, arguments);
                var scriptResultText = JSON.stringify(scriptResult);
                return JsonConvert.DeserializeObject<T>(scriptResultText);
            }

            throw new NotImplementedException();
        }
    }
}
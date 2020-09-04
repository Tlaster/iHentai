using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task<IEnumerable<IGallery>> Home(int page)
        {
            const string functionName = "home";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {page});
        }

        public async Task<IEnumerable<IGallery>> UserTimeline(int page)
        {
            const string functionName = "userTimeline";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {page});
        }

        public async Task<IEnumerable<IGallery>> Search(string keyword, int page)
        {
            const string functionName = "search";
            return await InvokeAsync<List<ScriptGalleryModel>>(functionName, new Arguments {keyword, page});
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
        
        async Task<IGalleryDetail> IDetailedApi.Detail(IGallery gallery)
        {
            return await Detail(gallery);
        }

        public async Task<bool> CheckCanOpenChapter(IMangaChapter chapter)
        {
            const string functionName = "canReadChapter";
            return await InvokeAsync<bool>(functionName, new Arguments {JSON.parse(JsonConvert.SerializeObject(chapter))});
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
            return await InvokeAsync<List<string>>(functionName, new Arguments {JSON.parse(JsonConvert.SerializeObject(chapter))});
        }

        public async Task<List<string>> GalleryImages(IGalleryDetail detail)
        {
            const string functionName = "loadGalleryImages";
            return await InvokeAsync<List<string>>(functionName, new Arguments {JSON.parse(JsonConvert.SerializeObject(detail))});
        }


        //public ReadingViewModel? GenerateReadingViewModel(IGallery gallery)
        //{
        //    throw new NotImplementedException();
        //}

        public bool CanHandle(Uri uri)
        {
            const string functionName = "canModifyRequest";
            if (_manifest.Hosts != null && _manifest.Hosts.Contains(uri.Host) && _engine.HasMember(functionName))
            {
                return _engine.InvokeFunction<bool>(functionName, new Arguments{uri.ToString()});
            }

            return false;
        }

        public void Handle(HttpRequestMessage message)
        {
            const string functionName = "modifyRequest";
            if (_engine.HasMember(functionName))
            {
                var content = Invoke<ScriptRequestContent>(functionName,
                    new Arguments
                    {
                        JSON.parse(JsonConvert.SerializeObject(ScriptRequestContent.FromHttpRequestMessage(message)))
                    });
                content.ToHttpRequestMessage(message);
            }
        }
        
        private T Invoke<T>(string name, Arguments arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = _engine.InvokeFunction<JSValue>(name, arguments);
                var scriptResultText = JSON.stringify(scriptResult);
                return JsonConvert.DeserializeObject<T>(scriptResultText);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        
        private async Task<T> InvokeAsync<T>(string name, Arguments arguments)
        {
            if (_engine.HasMember(name))
            {
                var scriptResult = await _engine.InvokeFunctionAsync<JSValue>(name, arguments);
                var scriptResultText = JSON.stringify(scriptResult);
                return JsonConvert.DeserializeObject<T>(scriptResultText);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
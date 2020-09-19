using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using iHentai.Common;
using iHentai.Extensions.Common;
using iHentai.Extensions.Hosting;
using iHentai.Extensions.Models;
using iHentai.Extensions.Runtime;
using iHentai.Scripting.Runtime;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Console = iHentai.Scripting.Runtime.Console;

namespace iHentai.Extensions
{
    public class ScriptEngine
    {
        private readonly string _extensionId;
        private readonly Fetch _fetch;
        private readonly JavaScriptNativeFunction _fetchInternal;
        private readonly ChakraHost _host;
        private readonly ExtensionManifest _manifest;

        public ScriptEngine(string extensionId, ExtensionManifest manifest)
        {
            _extensionId = extensionId;
            _manifest = manifest;
            _fetch = new Fetch(HentaiHttpHandler.Instance);
            _host = new ChakraHost();
            _host.EnterContext();
            _fetchInternal = FetchInternal;
        }

        public JsJson JSON => _host.JSON;

        public async Task Init(string path)
        {
            if (_manifest.Entry == null)
            {
                throw new ArgumentNullException("Manifest should have entry");
            }

            var filePath = Path.Combine(path, _manifest.Entry);
            var file = await StorageFile.GetFileFromPathAsync(filePath);
            var entryFile = await FileIO.ReadTextAsync(file);

            _host.DefineProperty("console", new Console());
            _host.DefineProperty("localStorage",
                new LocalStorage(_extensionId, HentaiApp.Instance.Resolve<IExtensionStorage>()));
            _host.DefineProperty("runtime", new RootRuntime());
            _host.GlobalObject.SetProperty(JavaScriptPropertyId.FromString("fetch"),
                JavaScriptValue.CreateFunction(_fetchInternal, IntPtr.Zero), true);
            _host.RunScript(@"
this.parseHtml = runtime.parseHtml;
this.unpack = runtime.unpack;
this.decodeLzStringFromBase64 = runtime.decodeLzStringFromBase64;
");
            _host.RunScript(entryFile);
        }


        private JavaScriptValue FetchInternal(JavaScriptValue callee, bool call,
            JavaScriptValue[] arguments,
            ushort count, IntPtr data)
        {
            var args = arguments.Skip(1).ToArray();
            if (args.Length == 0)
            {
                return JavaScriptValue.Invalid;
            }

            var urlJavaScriptValue = args.FirstOrDefault();
            if (urlJavaScriptValue.ValueType != JavaScriptValueType.String)
            {
                return JavaScriptValue.Invalid;
            }

            var url = urlJavaScriptValue.ToString();

            if (args.Length == 1)
            {
                var result = _fetch.fetch(url, null);
                Native.ThrowIfError(Native.JsInspectableToObject(result, out var jsResult));
                return jsResult;
            }
            else
            {
                var json = JSON.Stringify(args[1]);
                var init = json.Let(JsonConvert.DeserializeObject<FetchInit>);
                var result = _fetch.fetch(url, init);
                Native.ThrowIfError(Native.JsInspectableToObject(result, out var jsResult));
                return jsResult;
            }
        }

        public async Task<T> InvokeFunctionAsync<T>(string name, params object[] arguments)
        {
            var func = _host.GlobalObject.GetProperty(JavaScriptPropertyId.FromString(name));
            if (func.ValueType == JavaScriptValueType.Function)
            {
                var rawResult = func.CallFunction(new[] {_host.GlobalObject}
                    .Concat(arguments.Select(it => it.ToJavaScriptValue())).ToArray());
                JavaScriptValue result;
                if (IsPromise(rawResult))
                {
                    result = await PromiseToTask(rawResult);
                }
                else
                {
                    result = rawResult;
                }

                var json = JSON.Stringify(result);
                if (json is T tvalue)
                {
                    return tvalue;
                }

                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        private Task<JavaScriptValue> PromiseToTask(JavaScriptValue value)
        {
            return Task.Factory.FromAsync((callback, state) =>
            {
                var result = new AsyncResult();

                JavaScriptValue FulfilledCallback(JavaScriptValue callee, bool call, JavaScriptValue[] arguments,
                    ushort count, IntPtr data)
                {
                    result.SetResult(arguments.Skip(1).FirstOrDefault());
                    callback?.Invoke(result);
                    return JavaScriptValue.Invalid;
                }

                JavaScriptValue RejectCallback(JavaScriptValue callee, bool call, JavaScriptValue[] arguments,
                    ushort count, IntPtr data)
                {
                    var json = arguments[1].ConvertToString().ToString();
                    result.SetError(json);
                    callback?.Invoke(result);
                    return JavaScriptValue.Invalid;
                }


                var thenProperty = value.GetProperty(JavaScriptPropertyId.FromString("then"));
                thenProperty.CallFunction(value, JavaScriptValue.CreateFunction(FulfilledCallback, IntPtr.Zero),
                    JavaScriptValue.CreateFunction(RejectCallback, IntPtr.Zero));
                return result;
            }, result =>
            {
                var asyncResult = result as AsyncResult ?? throw new ArgumentException("Result is of wrong type.");
                if (asyncResult.HasError)
                {
                    throw new Exception(asyncResult.Error);
                }

                return asyncResult.Result;
            }, null);
        }


        private bool IsPromise(JavaScriptValue value)
        {
            if (!value.HasProperty(JavaScriptPropertyId.FromString("constructor")))
            {
                return false;
            }

            var constructor = value.GetProperty(JavaScriptPropertyId.FromString("constructor"));
            if (!constructor.HasProperty(JavaScriptPropertyId.FromString("name")))
            {
                return false;
            }

            var name = constructor.GetProperty(JavaScriptPropertyId.FromString("name"));
            return name.ValueType == JavaScriptValueType.String && name.ToString() == "Promise";
        }

        public async Task<T> InvokeFunction<T>(string name, params object[] arguments)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync(delegate
            {
                var func = _host.GlobalObject.GetProperty(JavaScriptPropertyId.FromString(name));
                if (func.ValueType == JavaScriptValueType.Function)
                {
                    var result = func.CallFunction(new[] {_host.GlobalObject}
                        .Concat(arguments.Select(it => it.ToJavaScriptValue())).ToArray());
                    var json = JSON.Stringify(result);
                    if (json is T tvalue)
                    {
                        return tvalue;
                    }

                    return JsonConvert.DeserializeObject<T>(json);
                }

                return default;
            });
        }

        public async Task<bool> HasMember(string name)
        {
            return await DispatcherHelper.ExecuteOnUIThreadAsync(delegate
            {
                var globalObject = _host.GlobalObject;
                var id = name.ToJavaScriptPropertyId();
                return globalObject.HasProperty(id);
            });
        }
    }
}
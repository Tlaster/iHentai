using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Toolkit.Uwp.Helpers;

namespace iHentai.Extensions.Hosting
{
    public class ChakraHost : IDisposable
    {
        private static JavaScriptSourceContext _currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        private static JavaScriptRuntime _runtime;
        private static readonly Queue<JavaScriptValue> TaskQueue =
            new Queue<JavaScriptValue>();
        private readonly CancellationTokenSource _shutdownCts = new CancellationTokenSource();
        private JavaScriptContext _context;
        private bool _isPromiseLooping;

        public ChakraHost()
        {
            Native.ThrowIfError(Native.JsCreateRuntime(JavaScriptRuntimeAttributes.EnableExperimentalFeatures, null,
                out _runtime));
            Native.ThrowIfError(Native.JsCreateContext(_runtime, out _context));
            EnterContext();
            Native.ThrowIfError(Native.JsSetPromiseContinuationCallback(PromiseContinuationCallback, IntPtr.Zero));
            //Native.ThrowIfError(Native.JsProjectWinRTNamespace("Windows"));
            Native.ThrowIfError(Native.JsGetGlobalObject(out var global));
            GlobalObject = global;
            JSON = new JsJson(global.GetProperty(JavaScriptPropertyId.FromString("JSON")), global);
#if DEBUG
            Native.ThrowIfError(Native.JsStartDebugging());
#endif
            LeaveContext();
        }

        public JsJson JSON { get; }

        public JavaScriptValue GlobalObject { get; }

        public void Dispose()
        {
            _shutdownCts.Cancel();
            _context.Release();
            _runtime.Dispose();
        }


        public void EnterContext()
        {
            Native.ThrowIfError(Native.JsSetCurrentContext(_context));
        }

        public void LeaveContext()
        {
            Native.ThrowIfError(Native.JsSetCurrentContext(JavaScriptContext.Invalid));
        }

        public void DefineProperty(string name, object obj)
        {
            Native.ThrowIfError(Native.JsInspectableToObject(obj, out var value));
            DefineHostProperty(name, value);
        }

        private void DefineHostProperty(string callbackName, JavaScriptValue value)
        {
            var propertyId = JavaScriptPropertyId.FromString(callbackName);
            GlobalObject.SetProperty(propertyId, value, true);
            Native.ThrowIfError(Native.JsAddRef(value, out _));
        }

        //public void WithContext(Action action)
        //{
        //    EnterContext();
        //    action.Invoke();
        //    LeaveContext();
        //}

        //public T WithContext<T>(Func<T> action)
        //{
        //    EnterContext();
        //    var result = action.Invoke();
        //    LeaveContext();
        //    return result;
        //}

        public JavaScriptValue RunScript(string script)
        {
            Native.ThrowIfError(Native.JsRunScript(script, _currentSourceContext++, "", out var result));
            return result;
        }

        private void PromiseContinuationCallback(JavaScriptValue task, IntPtr callbackState)
        {
            TaskQueue.Enqueue(task);
            task.AddRef();
            StartPromiseTaskLoop();
        }

        private void StartPromiseTaskLoop()
        {
            if (_isPromiseLooping)
            {
                return;
            }

            _isPromiseLooping = true;
            while (TaskQueue.Count != 0)
            {
                try
                {
                    var task = TaskQueue.Dequeue();
                    task.CallFunction(GlobalObject);
                    task.Release();
                }
                catch (OperationCanceledException e)
                {
                    return;
                }
            }

            _isPromiseLooping = false;
        }
    }

    public class JsJson
    {
        private readonly JavaScriptValue _global;
        private JavaScriptValue _json;

        public JsJson(JavaScriptValue json, JavaScriptValue global)
        {
            _json = json;
            _global = global;
        }

        public string Stringify(JavaScriptValue value)
        {
            return _json.GetProperty(JavaScriptPropertyId.FromString("stringify"))
                .CallFunction(_global, value).ConvertToString().ToString();
        }

        public JavaScriptValue Parse(string value)
        {
            return _json.GetProperty(JavaScriptPropertyId.FromString("parse"))
                .CallFunction(_global, JavaScriptValue.FromString(value));
        }
    }
}
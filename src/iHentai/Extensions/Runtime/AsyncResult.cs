using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iHentai.Extensions.Hosting;

namespace iHentai.Extensions.Runtime
{
    public class AsyncResult : IAsyncResult
    {
        private readonly ManualResetEvent _mre = new ManualResetEvent(false);

        public string Error { get; private set; }
        public bool HasError { get; private set; }
        public JavaScriptValue Result { get; private set; }

        public WaitHandle AsyncWaitHandle => _mre;

        public bool CompletedSynchronously { get; } = false;

        public bool IsCompleted { get; private set; }

        public object AsyncState => throw new NotImplementedException();

        public void SetError(string error)
        {
            Error = error;
            HasError = true;
            Release();
        }

        public void SetResult(JavaScriptValue value)
        {
            Result = value;
            Release();
        }

        private void Release()
        {
            IsCompleted = true;
            _mre.Set();
        }
    }
}

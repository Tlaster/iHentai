using System.Diagnostics;

namespace iHentai.Extensions.Runtime
{
    public class Log
    {
        private readonly string _extensionId;

        public Log(string extensionId)
        {
            _extensionId = extensionId;
        }

        public void log(object value)
        {
            Debug.WriteLine(_extensionId + ":" + value);
        }
    }
}
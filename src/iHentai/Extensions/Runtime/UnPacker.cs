using Jint;

namespace iHentai.Extensions.Runtime
{
    internal class UnPacker
    {
        public static string? Unpack(string script)
        {
            var engine = new Engine();
            var result = engine.Execute(script).GetCompletionValue().ToObject()?.ToString();
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;

namespace iHentai.Extensions.Runtime
{
    class UnPacker
    {
        public static string? Unpack(string script)
        {
            var engine = new Engine();
            var result = engine.Execute(script).GetCompletionValue().ToObject()?.ToString();
            return result;
        }
    }
}

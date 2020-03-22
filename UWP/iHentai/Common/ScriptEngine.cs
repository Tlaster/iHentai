using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHosting;

namespace iHentai.Common
{
    class ScriptEngine
    {
        private readonly ChakraHost _host;
        public ScriptEngine()
        {
            _host = new ChakraHost();
        }

        public string RunScript(string script)
        {
            _host.EnterContext();
            var result = _host.RunScript(script).ToString();
            _host.LeaveContext();
            return result;
        }
    }
}

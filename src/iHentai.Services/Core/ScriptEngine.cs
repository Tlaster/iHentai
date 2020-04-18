using Jint;

namespace iHentai.Services.Core
{
    class ScriptEngine
    {
        public static ScriptEngine Instance { get; } = new ScriptEngine();
        private readonly Engine _engine;

        private ScriptEngine()
        {
            _engine = new Engine();
        }

        public string RunScript(string script)
        {
            return _engine.Execute(script).GetCompletionValue().ToString();
        }
    }
}

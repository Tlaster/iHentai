using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHentai.Scripting.Runtime
{
    public sealed class Console
    {
        public void log(string value)
        {
            Debug.WriteLine(value);
        }
    }
}

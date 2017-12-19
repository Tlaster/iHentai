using System.Collections.Generic;
using System.Reflection;

namespace iHentai.Mvvm
{
    public interface IMvvmApplication
    {
        IEnumerable<Assembly> MvvmViewAssemblies();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHentai.Mvvm
{
    public interface IMvvmApplication
    {
        IEnumerable<Assembly> MvvmViewAssemblies();
    }
}

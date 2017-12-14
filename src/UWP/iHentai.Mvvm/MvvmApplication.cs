using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace iHentai.Mvvm
{
    public abstract class MvvmApplication : Application
    {
        public virtual IEnumerable<Assembly> MvvmViewAssemblies()
        {
            yield return Current.GetType().GetTypeInfo().Assembly;
        }
    }
}

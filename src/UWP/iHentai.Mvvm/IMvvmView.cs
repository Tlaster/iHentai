using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using iHentai.Paging;
using iHentai.Shared.Helpers;

namespace iHentai.Mvvm
{
    public interface IMvvmView<T> where T : ViewModel
    {
        T ViewModel { get; set; }
    }
}
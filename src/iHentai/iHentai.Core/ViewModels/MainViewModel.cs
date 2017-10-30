using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using iHentai.Core.Common;
using iHentai.Core.Views;
using iHentai.Mvvm;
using iHentai.Services.Core;

namespace iHentai.Core.ViewModels
{
    [Page(typeof(MainPage))]
    public class MainViewModel : ViewModel
    {
        private IHentaiApis _apis;

        public MainViewModel()
        {
        }

        public MainViewModel(IHentaiApis apis)
        {
            _apis = apis;
        }

        public IncrementalLoadingCollection<MyClass, string> ListSource { get; set; } =
            new IncrementalLoadingCollection<MyClass, string>();
        public bool IsLoading { get; set; }

        public ICommand RefreshCommand => new RelayCommand(() => { });
    }

    public class MyClass : IIncrementalSource<string>
    {
        public async Task<IEnumerable<string>> GetPagedItemsAsync(int pageIndex, CancellationToken cancellationToken = default)
        {
            await Task.Delay(2000, cancellationToken);
            return new List<string>();
            //return Enumerable.Range((pageIndex) * 20, (pageIndex + 1) * 20).Select(item => item.ToString());
        }
    }
}
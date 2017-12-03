using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace iHentai.Paging.Handlers
{
    internal class PageStateHandler
    {
        private readonly HentaiPage _page;
        private bool _stateLoaded;

        public PageStateHandler(HentaiPage page, string pageKey)
        {
            _page = page;
            PageKey = pageKey;
        }

        public string PageKey { get; internal set; }


        public void OnNavigatedTo(HentaiNavigationEventArgs e)
        {
            if (!_stateLoaded)
            {
                var frameState = HentaiSuspensionManager.SessionStateForFrame(_page.Frame);

                if (e.NavigationMode == NavigationMode.New)
                {
                    var nextPageKey = PageKey;
                    var nextPageIndex = _page.Frame.BackStackDepth;
                    while (frameState.Remove(nextPageKey))
                    {
                        nextPageIndex++;
                        nextPageKey = "Page" + nextPageIndex;
                    }
                }
                else
                {
                    var pageState = (Dictionary<string, object>) frameState[PageKey];

                    _page.OnLoadState(pageState);
                }

                _stateLoaded = true;
            }
        }

        public void OnNavigatedFrom(HentaiNavigationEventArgs e)
        {
            if (_page.Frame.DisableForwardStack && e.NavigationMode == NavigationMode.Back)
                return;

            var frameState = HentaiSuspensionManager.SessionStateForFrame(_page.Frame);
            var pageState = new Dictionary<string, object>();
            _page.OnSaveState(pageState);

            frameState[PageKey] = pageState;
        }
    }
}
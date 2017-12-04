using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;

namespace iHentai.Paging.Handlers
{
    public class BackKeyPressedHandler
    {
        private readonly List<Tuple<HentaiPage, Func<object, bool>>> _handlers;

        //private Type _hardwareButtonsType = null;
        //private object _registrationToken;
        private bool _isEventRegistered;

        public BackKeyPressedHandler()
        {
            _handlers = new List<Tuple<HentaiPage, Func<object, bool>>>();
        }


        public void Add(HentaiPage page, Func<object, bool> handler)
        {
            if (!_isEventRegistered)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackKeyPressed;
                _isEventRegistered = true;
            }

            _handlers.Insert(0, new Tuple<HentaiPage, Func<object, bool>>(page, handler));
        }


        public void Remove(HentaiPage page)
        {
            _handlers.Remove(_handlers.Single(h => h.Item1 == page));

            if (_handlers.Count == 0)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackKeyPressed;
                _isEventRegistered = false;
            }
        }


        private void OnBackKeyPressed(object sender, BackRequestedEventArgs args)
        {
            var handled = args.Handled;
            if (handled)
                return;

            foreach (var item in _handlers)
            {
                handled = item.Item2(sender);
                args.Handled = handled;
                if (handled)
                    return;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Core;
using iHentai.Basic.Helpers;
using iHentai.Mvvm;
using iHentai.Services;

namespace Conet.ViewModels
{
    [Startup]
    public class HomeViewModel : ViewModel
    {
        private readonly Guid _data;
        private readonly Guid _messageGuid;

        public HomeViewModel(string serviceType, Guid data)
        {
            _data = data;
            ServiceType = serviceType;
            _messageGuid = Guid.NewGuid();
            Singleton<MessagingCenter>.Instance.Subscribe<ConetActionArgs>(this, $"{ConetActionArgs.ConetAction}:{_messageGuid}", args =>
            {
                switch (args.Action)
                {
                    case ActionTypes.Account:
                        break;
                    case ActionTypes.Detail:
                        break;
                    case ActionTypes.Follower:
                        break;
                    case ActionTypes.Following:
                        break;
                    case ActionTypes.Custom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
            Source = serviceType.Get<IConetApi>().GetHomeContent(_data, _messageGuid).ToList();
        }

        protected override void OnDestory()
        {
            base.OnDestory();
            Singleton<MessagingCenter>.Instance.Unsubscribe(this, $"{ConetActionArgs.ConetAction}:{_messageGuid}");
        }

        public List<IConetViewModel> Source { get; }

        public string ServiceType { get; }

        protected override void OnStart()
        {
            base.OnStart();
            Frame.ClearBackStack();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using iHentai.Basic.Controls;
using iHentai.Basic.Helpers;
using iHentai.Services;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core
{
    public interface IConetApi : IApi, ICanLogin
    {
        Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20, long cursor = 0L);

        Task<(long Cursor, IEnumerable<JToken> Data)> UserTimeline(IInstanceData data, int count, long cursor = 0L);

        Task<JToken> User(IInstanceData data, string uid);

        IEnumerable<IConetViewModel> GetHomeContent(Guid data, Guid messageGuid);
    }

    public abstract class ConetViewModelBase : IConetViewModel, INotifyPropertyChanged
    {
        protected readonly Guid _data;
        protected readonly Guid _messageGuid;

        protected ConetViewModelBase(Guid messageGuid, Guid data)
        {
            _messageGuid = messageGuid;
            _data = data;
        }

        public abstract string Title { get; }

        public abstract Icons Icon { get; }

        public abstract int Badge { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Action(ActionTypes action, params object[] param)
        {
            Singleton<MessagingCenter>.Instance.Send($"{ConetActionArgs.ConetAction}:{_messageGuid}",
                new ConetActionArgs(action, param));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IConetViewModel
    {
        string Title { get; }
        Icons Icon { get; }
        int Badge { get; }
    }
}
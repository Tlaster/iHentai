using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using iHentai.Services;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core
{
    public interface IConetApi : IApi, ICanLogin
    {
        Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20, long cursor = 0L);

        Task<JToken> User(IInstanceData data, string uid);

        IEnumerable<(IConetViewModel ViewModel, UIElement Content)> GetNotificationContent();
    }

    public interface IConetViewModel
    {
        
    }
}
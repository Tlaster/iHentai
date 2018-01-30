using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using iHentai.Basic.Controls;
using iHentai.Services;
using Newtonsoft.Json.Linq;

namespace Conet.Apis.Core
{
    public interface IConetApi : IApi, ICanLogin
    {
        Task<(long Cursor, IEnumerable<JToken> Data)> HomeTimeline(IInstanceData data, int count = 20, long cursor = 0L);

        Task<(long Cursor, IEnumerable<JToken> Data)> UserTimeline(IInstanceData data, int count, long cursor = 0L);

        Task<JToken> User(IInstanceData data, string uid);

        IEnumerable<IConetViewModel> GetHomeContent(IInstanceData data);
    }

    public interface IConetViewModel
    {
        string Title { get; }
        Icons Icon { get; }
        int Badge { get; }
    }
}
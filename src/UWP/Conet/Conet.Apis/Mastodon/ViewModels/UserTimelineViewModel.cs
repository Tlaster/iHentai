using System;
using Conet.Apis.Core;
using iHentai.Basic.Controls;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Controls;
using iHentai.Services;
using Newtonsoft.Json.Linq;
using Nito.Mvvm;

namespace Conet.Apis.Mastodon.ViewModels
{
    public class UserTimelineViewModel : ConetViewModelBase
    {
        public UserTimelineViewModel(string uid)
        {
            Source = new AutoList<UserTimelineDataSource, JToken>(new UserTimelineDataSource(nameof(Mastodon)));
            UserSource = NotifyTask.Create(nameof(Mastodon).Get<Apis>().User(Singleton<ApiContainer>.Instance.CurrentInstanceData, uid));
            if ((Singleton<ApiContainer>.Instance.CurrentInstanceData as InstanceData).Uid != uid)
                RelationshipSource =
                    NotifyTask.Create(nameof(Mastodon).Get<Apis>().Relationship(Singleton<ApiContainer>.Instance.CurrentInstanceData as InstanceData, uid));
        }

        public NotifyTask<JToken> RelationshipSource { get; }

        public NotifyTask<JToken> UserSource { get; }

        public AutoList<UserTimelineDataSource, JToken> Source { get; }

        public override string Title { get; } = "my".GetLocalized();
        public override Icons Icon { get; } = Icons.Accounts;
        public override int Badge { get; set; }
    }
}
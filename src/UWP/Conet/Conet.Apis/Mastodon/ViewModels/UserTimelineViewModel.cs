using System;
using Conet.Apis.Core;
using iHentai.Basic.Controls;
using iHentai.Basic.Extensions;
using iHentai.Basic.Helpers;
using iHentai.Services;
using Newtonsoft.Json.Linq;
using Nito.Mvvm;

namespace Conet.Apis.Mastodon.ViewModels
{
    public class UserTimelineViewModel : ConetViewModelBase
    {
        public UserTimelineViewModel(string uid, Guid messageGuid, Guid data) : base(messageGuid, data)
        {
            Source = new AutoList<UserTimelineDataSource, JToken>(new UserTimelineDataSource(_data, nameof(Mastodon)));
            UserSource = NotifyTask.Create(nameof(Mastodon).Get<Apis>().User(data.Get<IInstanceData>(), uid));
            RelationshipSource =
                NotifyTask.Create(nameof(Mastodon).Get<Apis>().Relationship(data.Get<InstanceData>(), uid));
        }

        public NotifyTask<JToken> RelationshipSource { get; }

        public NotifyTask<JToken> UserSource { get; }

        public AutoList<UserTimelineDataSource, JToken> Source { get; }

        public override string Title { get; } = "my".GetLocalized();
        public override Icons Icon { get; } = Icons.Accounts;
        public override int Badge { get; set; }
    }
}

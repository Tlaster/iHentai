using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Mastodon.Models;
using iHentai.Basic.Extensions;
using iHentai.Database;
using iHentai.Mvvm;
using iHentai.Services;
using Mastodon.Api;
using Mastodon.Model;
using Nito.Mvvm;

namespace Conet.Mastodon.ViewModels
{
    [Startup]
    public class AccountSelectionViewModel : ViewModel
    {
        public List<NotifyTask<Account>> AccountList { get; set; }

        public AccountSelectionViewModel(string service)
        {
            InitAccounts();
        }

        public void InitAccounts()
        {
            using (var context = new ApplicationDbContext())
            {
                var datas = context.Instances.Where(item => item.Service == nameof(Mastodon))
                    .Select(item => item.Data.JsonTo<InstanceData>()).ToList();
                AccountList = datas.Select(item => NotifyTask.Create(RefreshAccount(item))).ToList();
            }
        }

        private Task<Account> RefreshAccount(InstanceData item)
        {
            return Accounts.Fetching(item.Domain, item.Uid, item.AccessToken);
        }

        public void Add()
        {

        }
    }
}

using System.Collections.Generic;
using iHentai.Basic.Extensions;

namespace Conet.Apis.Core
{
    public enum ServiceTypes
    {
        Mastodon,
        Weibo,
        Twitter
    }

    public class GlobalSettings
    {
        public List<AccountModel> Accounts
        {
            get => "conet_accounts".Read(new List<AccountModel>());
            set => value.Save("conet_accounts");
        }

        //public int SelectedAccountIndex
        //{
        //    get => "conet_account_index".Read(0);
        //    set => value.Save("conet_account_index");
        //}
    }

    public class AccountModel
    {
        public Dictionary<string, string> CustomData { get; set; }
        public string AccessToken { get; set; }
        public ServiceTypes Service { get; set; }
    }
}
using System.Threading.Tasks;
using Mastodon.Common;
using Mastodon.Model;

namespace Mastodon.Api
{
    public static class Lists
    {
        public static async Task<MastodonList<List>> GetLists(string domain, string token)
        {
            return await HttpHelper.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.List}", token);
        }

        public static async Task<MastodonList<List>> ListsByMembership(string domain, string token)
        {
            return await HttpHelper.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.ListsByMembership}",
                token);
        }

        public static async Task<MastodonList<List>> AccountsInList(string domain, string token)
        {
            return await HttpHelper.GetListAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token);
        }

        public static async Task<List> ListById(string domain, string token, string id)
        {
            return await HttpHelper.GetAsync<List>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }

        public static async Task<List> CreateList(string domain, string token, string title)
        {
            return await HttpHelper.PostAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.List}", token,
                (nameof(title), title));
        }

        public static async Task<List> UpdateList(string domain, string token, string id)
        {
            return await HttpHelper.PutAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }
        
        public static async Task<List> DeleteList(string domain, string token, string id)
        {
            return await HttpHelper.DeleteAsync<List, string>($"{HttpHelper.HTTPS}{domain}{Constants.ListById}", token,
                param: (nameof(id), id));
        }

        public static async Task AddAccount(string domain, string token, string id)
        {
            await HttpHelper.PostAsync($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token,
                (nameof(id), id));
        }
        
        public static async Task RemoveAccount(string domain, string token, string id)
        {
            await HttpHelper.DeleteAsync($"{HttpHelper.HTTPS}{domain}{Constants.AccountsInList}", token,
                (nameof(id), id));
        }
        
    }
}
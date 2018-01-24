using iHentai.Services;

namespace Conet.Apis.Mastodon
{
    [Equals]
    public class InstanceData : IInstanceData
    {
        public InstanceData()
        {
        }

        public InstanceData(string accessToken, string domain, string uid)
        {
            AccessToken = accessToken;
            Domain = domain;
            Uid = uid;
        }

        public string AccessToken { get; set; }
        public string Uid { get; set; }
        public string Domain { get; set; }
    }
}
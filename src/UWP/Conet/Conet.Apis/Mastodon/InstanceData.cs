using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conet.Apis.Mastodon.Model.OAuth;
using iHentai.Services;

namespace Conet.Apis.Mastodon
{
    public class InstanceData : TokenModel, IInstanceData
    {
        public InstanceData()
        {
            
        }
        public InstanceData(TokenModel tokenModel, string domain, long uid)
        {
            AccessToken = tokenModel.AccessToken;
            TokenType = tokenModel.TokenType;
            ExpiresIn = tokenModel.ExpiresIn;
            RefreshToken = tokenModel.RefreshToken;
            CreatedAt = tokenModel.CreatedAt;
            Scope = tokenModel.Scope;
            Domain = domain;
            Uid = uid;
        }

        public long Uid { get; set; }
        public string Domain { get; set; }
    }
}

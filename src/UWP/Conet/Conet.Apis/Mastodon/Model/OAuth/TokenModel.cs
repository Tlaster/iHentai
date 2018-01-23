using iHentai.Services;
using Newtonsoft.Json;

namespace Conet.Apis.Mastodon.Model.OAuth
{
    public class TokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        /// <summary>
        /// <see cref="AccessToken"/> will expire in <see cref="ExpiresIn"/> seconds
        /// </summary>
        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public int CreatedAt { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}

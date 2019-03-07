using System;
using Newtonsoft.Json;

namespace Client
{
    internal class OktaToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring => !string.IsNullOrEmpty(AccessToken) && ExpiresAt > DateTime.UtcNow.AddSeconds(30);
    }
}
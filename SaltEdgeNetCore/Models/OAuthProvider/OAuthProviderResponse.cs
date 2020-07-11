using System;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class OAuthProviderResponse
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("connection_secret")]
        public string ConnectionSecret { get; set; }

        [JsonProperty("attempt_id")]
        public string AttemptId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }
    }
}
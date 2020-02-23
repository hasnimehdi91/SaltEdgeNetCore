using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class ReconnectOAuthProvider: OAuthProvider
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }
    }
}
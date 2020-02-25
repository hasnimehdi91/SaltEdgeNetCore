using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class ReconnectOAuthProvider: SeOAuthProvider
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }
    }
}
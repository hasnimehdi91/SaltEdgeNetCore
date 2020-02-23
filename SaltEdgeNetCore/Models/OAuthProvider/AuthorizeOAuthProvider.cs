using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class AuthorizeOAuthProvider
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }

        [JsonProperty("query_string")]
        public string QueryString { get; set; }
    }
}
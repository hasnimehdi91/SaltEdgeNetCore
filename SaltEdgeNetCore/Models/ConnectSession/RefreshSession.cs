using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class RefreshSession
    {
        [JsonProperty("connection_id")]
        public string ConnectionId { get; set; }
    }
}
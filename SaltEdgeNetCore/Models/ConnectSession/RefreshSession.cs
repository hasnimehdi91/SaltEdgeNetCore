using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class RefreshSession : Session
    {
        [JsonProperty("connection_id")] 
        public string ConnectionId { get; set; }
    }
}
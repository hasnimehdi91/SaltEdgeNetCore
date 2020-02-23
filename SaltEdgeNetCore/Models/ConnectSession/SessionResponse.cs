using System;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.ConnectSession
{
    public class SessionResponse
    {
        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("connect_url")]
        public string ConnectUrl { get; set; }
    }
}
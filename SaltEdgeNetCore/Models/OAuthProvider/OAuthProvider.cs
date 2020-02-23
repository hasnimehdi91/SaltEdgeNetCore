using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Attempts;
using SaltEdgeNetCore.Models.Consents;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class OAuthProvider
    {
        [JsonProperty("attempt")]
        public Attempt Attempt { get; set; }
        
        [JsonProperty("consent")]
        public Consent Consent { get; set; }
        
        [JsonProperty("daily_refresh")]
        public bool? DailyRefresh { get; set; }
        
        [JsonProperty("return_connection_id")]
        public bool? ReturnConnectionId { get; set; }
        
        [JsonProperty("categorization")]
        public string Categorization { get; set; }
        
        [JsonProperty("include_fake_providers")]
        public bool? IncludeFakeProviders { get; set; }
    }
}
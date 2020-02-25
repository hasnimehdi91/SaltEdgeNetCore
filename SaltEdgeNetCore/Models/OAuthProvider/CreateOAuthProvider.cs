using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.OAuthProvider
{
    public class CreateOAuthProvider: SeOAuthProvider
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        
        [JsonProperty("provider_code")]
        public string ProviderCode { get; set; }
    }
}
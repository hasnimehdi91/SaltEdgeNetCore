using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class SeMerchantContact
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; } 
    }
}
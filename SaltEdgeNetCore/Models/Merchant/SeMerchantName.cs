using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class SeMerchantName
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
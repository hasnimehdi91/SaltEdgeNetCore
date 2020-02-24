using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Merchant
{
    public class Name
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
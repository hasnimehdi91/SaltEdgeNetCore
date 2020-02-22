using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Customer
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
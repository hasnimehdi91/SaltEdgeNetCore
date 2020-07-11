using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Customer
{
    public class UnlockCustomer
    {
        [JsonProperty("unlocked")]
        public bool Unlocked { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
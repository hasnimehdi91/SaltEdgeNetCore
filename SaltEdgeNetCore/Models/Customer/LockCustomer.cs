using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Customer
{
    public class LockCustomer
    {
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
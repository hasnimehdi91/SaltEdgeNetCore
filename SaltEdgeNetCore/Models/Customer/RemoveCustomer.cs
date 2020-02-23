using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Customer
{
    public class RemoveCustomer
    {
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
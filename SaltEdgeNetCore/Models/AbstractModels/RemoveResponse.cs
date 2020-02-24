using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.AbstractModels
{
    public abstract class RemoveResponse
    {
        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
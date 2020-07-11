using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.AbstractModels
{
    public abstract class RemoveResponse
    {
        [JsonProperty("removed")]
        public bool? Removed { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
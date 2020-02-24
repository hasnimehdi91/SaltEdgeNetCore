using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Assets
{
    public class Asset
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
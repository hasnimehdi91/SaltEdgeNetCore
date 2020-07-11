using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Assets
{
    public class SeAsset
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
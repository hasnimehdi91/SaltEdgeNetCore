using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Extra;

namespace SaltEdgeNetCore.Models.Provider
{
    public class SeInteractiveField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("english_name")]
        public string EnglishName { get; set; }

        [JsonProperty("localized_name")]
        public string LocalizedName { get; set; }

        [JsonProperty("nature")]
        public string Nature { get; set; }

        [JsonProperty("optional")]
        public bool Optional { get; set; }

        [JsonProperty("extra")]
        public SeAccountExtra AccountExtra { get; set; }

        [JsonProperty("position")]
        public int? Position { get; set; }
    }
}